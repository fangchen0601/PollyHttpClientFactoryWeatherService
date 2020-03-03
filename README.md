# PollyHttpClientFactoryWeatherService
refer to https://www.twilio.com/blog/more-resilient-service-communication-dotnet-polly-httpclientfactory

make request to Weather Service to test if Weather Service can retry calling Temperature service:
curl -i http://localhost:5001/weather/11

or make request to Temperatur service to see how Temperatur service is very unreliable:
curl -i http://localhost:6001/temperature/11
curl -i http://localhost:6001/temperature/11
curl -i http://localhost:6001/temperature/11
curl -i http://localhost:6001/temperature/11
