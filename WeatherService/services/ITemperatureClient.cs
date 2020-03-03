using System.Threading.Tasks;
using System.Net.Http;
using WeatherService.Models.Outgoing;

namespace WeatherService.services
{
    public interface ITemperatureClient
    {
        Task<HttpResponseMessage> Get(int locationId);
        Task<HttpResponseMessage> Post(TemperatureInfo info);
        Task<HttpResponseMessage> Delete(int locationId);
    }
}