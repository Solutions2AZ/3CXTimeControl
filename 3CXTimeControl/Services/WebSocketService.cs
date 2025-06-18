using _3CXTimeControl.Models;
using _3CXTimeControl.Models._3CX;
using _3CXTimeControl.Models.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Runtime;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace _3CXTimeControl.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_settings"></param>
    /// <param name="_logger"></param>
    /// <param name="_connectionService"></param>
    /// <param name="_webhookService"></param>
    /// <param name="_memoryStorageService"></param>
    /// <param name="_dbContext"></param>
    /// <param name="_taskService"></param>
    /// <param name="_config"></param>
    public class WebSocketService(Config3cxSettings _settings,
        ILogger<WebSocketService> _logger,
        ConnectionService _connectionService,
        MemoryStorageService _memoryStorageService,
        ControlLlamadasContext _dbContext,
        TaskSchedulerService _taskService,
        Config3cxSettings _config
        ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("WebSocketService is starting...");
            string domain = "wss://" + _settings.Domain + "/callcontrol/ws";
            while (!stoppingToken.IsCancellationRequested)
            {
                using var client = new ClientWebSocket();
                try
                {
                    // Obtener el token y configurar el cliente
                    var login = await _connectionService.Login();
                    client.Options.SetRequestHeader("Authorization", "Bearer " + login!.access_token);

                    _logger.LogInformation("Attempting to connect to WebSocket...");
                    await client.ConnectAsync(new Uri(domain), stoppingToken);
                    _logger.LogInformation($"Connected to {domain}");

                    var buffer = new byte[1024];
                    while (client.State == WebSocketState.Open && !stoppingToken.IsCancellationRequested)
                    {
                        try
                        {
                            var result = await client.ReceiveAsync(buffer, stoppingToken);
                            if (result.MessageType == WebSocketMessageType.Text)
                            {
                                var eventDate = new DateTimeOffset(DateTime.UtcNow);
                                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                                //_logger.LogInformation($"Received message: {message}");

                                var decoMessage = JsonSerializer.Deserialize<WebSocketRS>(message);
                                if (decoMessage != null && decoMessage._event != null)
                                {
                                    switch (decoMessage._event.event_type) //https://www.3cx.com/docs/call-control-api
                                    {
                                        case 0: //Upsert - The entity is either added or updated
                                            if (decoMessage._event.entity!.ToLower().Contains("/participants/"))
                                            {
                                                var id = int.Parse(decoMessage._event.entity!.Split("/")[4]);
                                                var extension = int.Parse(decoMessage._event.entity!.Split("/")[2]);
                                                var resp = await _connectionService.SendParticipantRequest(decoMessage._event.entity!);
                                                if (resp != null)
                                                {
                                                    var data = resp.toNotification();
                                                    data.date = eventDate;
                                                    var items = _memoryStorageService.GetItems(id);

                                                    if (!items.Any(x => data.Equals(x)))
                                                    {
                                                        _memoryStorageService.AddItem(data);
                                                    }
                                                    if ((extension >= _config.MinExtension && extension <= _config.MaxExtension) && resp != null && resp.status!.ToLower() == "connected")
                                                    {
                                                        var infoCliente = await _dbContext.Clientes.FirstOrDefaultAsync(x => x.NumeroTelefono == resp.party_caller_id);

                                                        if (infoCliente != null)
                                                        {
                                                            var taskId = _taskService.ProgramarTarea(
                                                                decoMessage._event.entity!, infoCliente.Id, decoMessage._event.entity!,
                                                                TimeSpan.FromMinutes(1)
                                                            );
                                                        }
                                                        else
                                                        {
                                                            if (string.IsNullOrEmpty(_config.CFDInitialNoTime))
                                                                await _connectionService.ActionOfCall(decoMessage._event.entity!, "drop");
                                                            else
                                                                await _connectionService.ActionOfCall(decoMessage._event.entity!, "routeto", _config.CFDInitialNoTime!);
                                                        }

                                                    }
                                                }

                                            }
                                            break;
                                        case 1: //Remove - The entity has been removed
                                            if (decoMessage._event.entity!.ToLower().Contains("/participants/"))
                                            {
                                                var id = int.Parse(decoMessage._event.entity!.Split("/")[4]);
                                                var extension = int.Parse(decoMessage._event.entity!.Split("/")[2]);
                                                var items = _memoryStorageService.GetItems(id);
                                                if (items.Any())
                                                {
                                                    var lastItem = items.OrderByDescending(x => x.date).FirstOrDefault()!;
                                                    lastItem.date = eventDate;
                                                    lastItem.status = getLastStatus(items);

                                                    // parametros la tarea y si la para hacemos el corte 
                                                    if (_taskService.DetenerTarea(decoMessage._event.entity!))
                                                    {
                                                        //Cogemos el cliente de nuevo, y si el tiempo es > 0 , le restamos 1
                                                        var cliente = await _dbContext.Clientes.FirstOrDefaultAsync(x => x.NumeroTelefono == lastItem.destination);
                                                        if (cliente != null)
                                                        {
                                                            cliente.MinutosDisponibles -= 1;
                                                            if (cliente.MinutosDisponibles <= 0)
                                                            {
                                                                cliente.MinutosDisponibles = 0;
                                                            }
                                                            cliente.FechaUltimaActualizacion = DateTime.Now;
                                                            DateTime fechaInicio = items.OrderBy(x => x.date).FirstOrDefault()!.date!.Value.DateTime;
                                                            DateTime fechaFin = lastItem.date.Value.DateTime;
                                                            TimeSpan diferencia = fechaFin - fechaInicio;
                                                            int minutosRedondeados = (int)Math.Ceiling(diferencia.TotalMinutes);
                                                            cliente.Llamada.Add(new Llamada() { DuracionMinutos = minutosRedondeados, FechaFin = fechaFin, FechaInicio = fechaInicio, ClienteId = cliente.Id, NumeroTelefono = lastItem.destination, MinutosConsumidos = minutosRedondeados });
                                                            await _dbContext.SaveChangesAsync();
                                                        }
                                                        _memoryStorageService.ClearItems(id);
                                                    }

                                                    
                                                }

                                            }


                                            break;
                                        case 2: //DTMFstring - DTMF provider by remote party

                                            break;
                                        case 4: //Response - Response to the request sent over WebSocket

                                            break;
                                        default:
                                            break;
                                    }
                                }

                            }
                            else if (result.MessageType == WebSocketMessageType.Close)
                            {
                                _logger.LogWarning("WebSocket is closing...");
                                await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", stoppingToken);
                            }
                            else
                            {
                                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                                _logger.LogInformation($"Received message: {message}");

                            }
                        }
                        catch (WebSocketException ex)
                        {
                            _logger.LogError(ex, "WebSocket receive error. Attempting to reconnect...");
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error connecting to WebSocket. Retrying...");
                }
                finally
                {
                    if (client.State == WebSocketState.Open)
                    {
                        await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", stoppingToken);
                        _logger.LogInformation("WebSocket closed.");
                    }
                }

                // Esperar antes de reconectar con lógica exponencial
                int reconnectDelay = 1000; // 1 segundo inicial
                const int maxReconnectDelay = 30000; // Máximo 30 segundos

                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation($"Reconnecting in {reconnectDelay / 1000} seconds...");
                    await Task.Delay(reconnectDelay, stoppingToken);

                    reconnectDelay = Math.Min(reconnectDelay * 2, maxReconnectDelay); // Incrementar exponencialmente
                    break; // Intentar reconectar después del retraso
                }
            }

            _logger.LogInformation("WebSocketService is stopping.");
        }


        private string? getLastStatus(List<NotificationRQ> items)
        {
            if (items.Any(x => x.status!.ToLower() == "connected"))
            {
                return "finished";
            }
            else
            {
                if (items.FirstOrDefault()!.isInbound)
                {
                    return "missed";
                }
                else
                    return "notanswered";
            }


        }
    }
}
