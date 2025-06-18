using _3CXTimeControl.Models;
using _3CXTimeControl.Models.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;

namespace _3CXTimeControl.Services
{
    /// <summary>
    /// TaskSchedulerService
    /// </summary>
    public class TaskSchedulerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConnectionService _connectionService;
        private readonly Config3cxSettings _config;
        private readonly ConcurrentDictionary<string, CancellationTokenSource> _tasks = new();
        /// <summary>
        /// TaskSchedulerService
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="connectionService"></param>
        /// <param name="config"></param>
        public TaskSchedulerService(IServiceProvider serviceProvider, ConnectionService connectionService, Config3cxSettings config)
        {
            _serviceProvider = serviceProvider;
            _connectionService = connectionService;
            _config = config;
        }
        /// <summary>
        /// ProgramarTarea
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="clienteId"></param>
        /// <param name="path"></param>
        /// <param name="intervalo"></param>
        /// <returns></returns>
        public string ProgramarTarea(string taskId, int clienteId, string path, TimeSpan intervalo)
        {
            if (!_tasks.Any(x => x.Key == taskId))
            {
                var cts = new CancellationTokenSource();
                _tasks.TryAdd(taskId, cts);
                _ = EjecutarTareaPeriodica(clienteId, path, intervalo, cts.Token);
            }

            return taskId;
        }

        private async Task EjecutarTareaPeriodica(int clienteId, string path, TimeSpan intervalo, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var db = scope.ServiceProvider.GetRequiredService<ControlLlamadasContext>();
                        var cliente = await db.Clientes.FirstOrDefaultAsync(x => x.Id == clienteId);
                        if (cliente != null)
                        {
                            cliente.MinutosDisponibles -= 1;

                            cliente.FechaUltimaActualizacion = DateTime.Now;
                            await db.SaveChangesAsync();
                            if (cliente.MinutosDisponibles < 0)
                            {
                                cliente.MinutosDisponibles = 0;
                                await db.SaveChangesAsync();
                                if (string.IsNullOrEmpty(_config.CFDEndCall))
                                    await _connectionService.ActionOfCall(path, "drop");
                                else
                                    await _connectionService.ActionOfCall(path, "routeto", _config.CFDEndCall!);

                            }
                            Console.WriteLine($"Cliente {clienteId}: MinutosDisponibles={cliente.MinutosDisponibles}, FechaUltimaActualizacion={cliente.FechaUltimaActualizacion}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error actualizando Cliente {clienteId}: {ex.Message}");
                }

                try
                {
                    await Task.Delay(intervalo, token);
                }
                catch (TaskCanceledException) { break; }
            }
        }

        public bool DetenerTarea(string taskId)
        {
            if (_tasks.TryRemove(taskId, out var cts))
            {
                cts.Cancel();
                cts.Dispose();
                return true;
            }
            return false;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.CompletedTask;
    }
}
