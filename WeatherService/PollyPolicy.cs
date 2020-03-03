// © Microsoft Corporation. All rights reserved.

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Polly;
using Polly.Extensions.Http;

namespace WeatherService
{
    public static class PollyPolicy
    {
        // Retry on following transient errors:
        // HttpStatusCode.ServiceUnavailable, (503)
        // HttpStatusCode.BadGateway, (502)
        // HttpStatusCode.GatewayTimeout, (504)
        // HttpStatusCode.RequestTimeout (408)
        public static readonly IAsyncPolicy<HttpResponseMessage> HttpWaitAndRetryPolicy =
                Policy.HandleResult<HttpResponseMessage>(r => ErrorCodesToRetry.Contains(r.StatusCode))
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt));

        private static readonly HttpStatusCode[] ErrorCodesToRetry =
        {
            HttpStatusCode.ServiceUnavailable,  //503
            HttpStatusCode.BadGateway,          //502
            HttpStatusCode.GatewayTimeout,      //504
            HttpStatusCode.RequestTimeout       //408
        };
    }
}
