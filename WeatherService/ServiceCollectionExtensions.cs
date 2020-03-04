using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherService.services;
using Polly;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace WeatherService
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMessageClient(this IServiceCollection serviceCollection, IConfigurationSection configSection)
        {
            serviceCollection.AddHttpClient<ITemperatureClient, TemperatureClient>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:6001/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            }).SetWawitAndRetryPolicy();
        }

        public static void SetWawitAndRetryPolicy(this IHttpClientBuilder clientBuilder)
        {
            clientBuilder.AddPolicyHandler((service, request) =>
                Policy.HandleResult<HttpResponseMessage>(r => ErrorCodesToRetry.Contains(r.StatusCode))
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt), onRetry: (response, calculatedWaitDuration) =>
                {
                    service.GetService<ILogger>().LogError($"Failed attempt. Waited for {calculatedWaitDuration}. " +
                        $"Retrying. {response.Exception.Message} - {response.Exception.StackTrace}");
                }));
        }

        private static readonly HttpStatusCode[] ErrorCodesToRetry =
        {
            HttpStatusCode.ServiceUnavailable,
            HttpStatusCode.BadGateway,
            HttpStatusCode.GatewayTimeout,
            HttpStatusCode.RequestTimeout,
            HttpStatusCode.InternalServerError
        };
    }
}
