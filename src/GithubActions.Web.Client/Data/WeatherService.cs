using System.Threading.Tasks;
using GithubActions.Contract.v1;
using GithubActions.Shared.Clients;

namespace GithubActions.Web.Client.Data
{
    public interface IWeatherService
    {
        Task<WeatherForecastResponse> GetAsync(string city);
    }

    public class WeatherService : IWeatherService
    {
        private readonly IApiClient _client;

        public WeatherService(IApiClient client)
        {
            _client = client;
        }

        public async Task<WeatherForecastResponse> GetAsync(string city)
        {
            var response = await _client.GetAsync(city);

            return response;
        }
    }
}