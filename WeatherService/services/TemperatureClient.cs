using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherService.Models.Outgoing;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace WeatherService.services
{
    public class TemperatureClient : ITemperatureClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<ITemperatureClient> _logger;

        public TemperatureClient(HttpClient httpClient)
        {
            _client = httpClient;
        }

        public async Task<HttpResponseMessage> Get(int locationId)
        {
            HttpResponseMessage httpResponseMessage = await _client.GetAsync($"temperature/{locationId}");
            return httpResponseMessage;
        }

        public async Task<HttpResponseMessage> Post(TemperatureInfo info)
        {
            string temperatureJson = JsonConvert.SerializeObject(info);
            HttpContent httpContent = new StringContent(temperatureJson, Encoding.UTF8, "application/json");

            var httpResponseMessage = await _client.PostAsync("temperature", httpContent);

            return httpResponseMessage;
        }

        public async Task<HttpResponseMessage> Delete(int locationId)
        {
            HttpResponseMessage httpResponseMessage = await _client.DeleteAsync($"temperature/{locationId}");

            return httpResponseMessage;
        }


    }
}
