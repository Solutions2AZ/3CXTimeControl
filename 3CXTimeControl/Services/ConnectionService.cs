using _3CXTimeControl.Models;
using _3CXTimeControl.Models._3CX;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace _3CXTimeControl.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="config"></param>
    public class ConnectionService(Config3cxSettings config)
    {
        public async Task<LoginRS?> Login()
        {
            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://" + config.Domain + "/connect/token");
            var collection = new List<KeyValuePair<string, string>>();
            collection.Add(new("client_id", config.ClientId!));
            collection.Add(new("client_secret", config.Secret!));
            collection.Add(new("grant_type", "client_credentials"));
            var content = new FormUrlEncodedContent(collection);
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LoginRS>(jsonResponse);
        }
        public async Task<ParticipantsRS?> SendParticipantRequest(string path)
        {
            try
            {
                using var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, "https://" + config.Domain + path);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", (await Login())!.access_token);

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string jsonResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine(jsonResponse);
                return JsonSerializer.Deserialize<ParticipantsRS>(jsonResponse);
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public async Task ActionOfCall(string path, string action = "drop", string destination = "")
        {
            try
            {
                using var client = new HttpClient();
                var url = "https://" + config.Domain + path + "/" + action;
                var request = new HttpRequestMessage(HttpMethod.Post, url);

                var body = new { reason = "Cortamos", destination = destination };
                string jsonBody = JsonSerializer.Serialize(body);

                request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", (await Login())!.access_token);

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string jsonResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine(jsonResponse);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar la solicitud: {ex.Message}");
            }
        }




    }
}
