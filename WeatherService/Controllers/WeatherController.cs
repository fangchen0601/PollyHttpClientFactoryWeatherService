﻿using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WeatherService.Models.Incoming;
using WeatherService.Models.Outgoing;
using WeatherService.services;

namespace WeatherService.Controllers
{
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        //private readonly HttpClient _httpClient;

        /*
        public WeatherController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        */

        /*
        private readonly IHttpClientFactory _httpClientFactory;

        public WeatherController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        */

        private readonly ITemperatureClient _temperatureClient;
        public WeatherController(ITemperatureClient temperatureClient)
        {
            _temperatureClient = temperatureClient;
        }

        [HttpGet("{locationId}")]
        public async Task<ActionResult> Get(int locationId)
        {
            //HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync($"temperature/{locationId}");

            /*
            var httpClient = _httpClientFactory.CreateClient("TemperatureService");
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"temperature/{locationId}");
            */

            HttpResponseMessage httpResponseMessage = await _temperatureClient.Get(locationId);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                int temperature = await httpResponseMessage.Content.ReadAsAsync<int>();
                return Ok(temperature);
            }

            return StatusCode((int)httpResponseMessage.StatusCode, "The temperature service returned an error.");
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] WeatherInfo weatherModel) 
        {
            var temperatureInfo = new TemperatureInfo
            {
                LocationId = weatherModel.LocationId,
                Temperature = weatherModel.Temperature,
                DateMeasured = weatherModel.DateTemperatureMeasured
            };

            var httpResponseMessage = await _temperatureClient.Post(temperatureInfo);

            /*
            string temperatureJson = JsonConvert.SerializeObject(temperatureInfo);
            HttpContent httpContent = new StringContent(temperatureJson, Encoding.UTF8, "application/json");

            var httpClient = _httpClientFactory.CreateClient("TemperatureService");
            var httpResponseMessage = await httpClient.PostAsync("temperature", httpContent);
            */

            //var httpResponseMessage = await _httpClient.PostAsync("temperature", httpContent);

            return StatusCode((int) httpResponseMessage.StatusCode);
        }

        [HttpDelete("{locationId}")]
        public async Task<ActionResult> Delete(int locationId)
        {
            /*
            var httpClient = _httpClientFactory.CreateClient("TemperatureService");
            HttpResponseMessage httpResponseMessage = await httpClient.DeleteAsync($"temperature/{locationId}");
            */

            //HttpResponseMessage httpResponseMessage = await _httpClient.DeleteAsync($"temperature/{locationId}");

            HttpResponseMessage httpResponseMessage = await _temperatureClient.Delete(locationId);

            return StatusCode((int)httpResponseMessage.StatusCode);
        }
    }
}
