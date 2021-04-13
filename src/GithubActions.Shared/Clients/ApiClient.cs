using System.Net.Http;
using System.Threading.Tasks;
using GithubActions.Contract;
using GithubActions.Contract.v1;
using GithubActions.Shared.Extensions;
using GithubActions.Shared.Logging;

namespace GithubActions.Shared.Clients
{
    public interface IApiClient
    {
        Task<WeatherForecastResponse> GetAsync(string city);
    }

    public class ApiClient : IApiClient
    {
        public const string Name = "API";
        private readonly ISimpleLogger _logger;
        private readonly ICustomHttpClient _client;

        public ApiClient(ISimpleLogger logger, IHttpClientFactory httpClientFactory, ICustomHttpClientFactory factory)
        {
            _logger = logger;

            var httpClient = httpClientFactory.CreateClient(Name);
            _client = factory.CreateClient(httpClient);

            Urls = new Urls(_client.BaseAddress, 1);
        }

        public Urls Urls { get; }

        public async Task<WeatherForecastResponse> GetAsync(string city)
        {
            var uri = Urls.WeatherForecast.Get(city);
            var httpResponse = await _client.GetAsync(uri);
            _logger.Info($"Response from HTTP GET '{uri}': '{httpResponse.StatusCode}'");
            return await httpResponse.GetContentAsAsync<WeatherForecastResponse>();
        }
    }
}