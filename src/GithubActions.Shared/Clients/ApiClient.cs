using System.Net.Http;
using System.Threading.Tasks;

namespace GithubActions.Shared.Clients
{
    public class ApiClient
    {
        public ApiClient(IHttpClientWrapper httpClientWrapper)
        {
            var baseUrl = string.Empty;

            Swagger = new Swagger(httpClientWrapper, baseUrl);
            V1 = new V1Client(httpClientWrapper, baseUrl);
            V2 = new V2Client(httpClientWrapper, baseUrl);
        }

        public Swagger Swagger { get; set; }
        public V1Client V1 { get; set; }
        public V2Client V2 { get; set; }
    }

    public class Swagger
    {
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly string _baseUrl;

        public Swagger(IHttpClientWrapper httpClientWrapper, string baseUrl)
        {
            _httpClientWrapper = httpClientWrapper;
            _baseUrl = string.Join('/', baseUrl, "swagger");
        }

        public async Task<HttpResponseMessage> GetJsonAsync(string version = "v1")
        {
            var url = string.Join('/', _baseUrl, version, "swagger.json");
            return await _httpClientWrapper.GetAsync($"{url}");
        }

        public async Task<HttpResponseMessage> GetUiAsync()
        {
            var url = string.Join('/', _baseUrl, "index.html");
            return await _httpClientWrapper.GetAsync($"{url}");
        }
    }

    public class V1Client
    {
        public V1Client(IHttpClientWrapper httpClientWrapper, string baseUrl)
        {
            var v2BaseUrl = string.Join('/', baseUrl, "v1");

            WeatherForeCast = new V1WeatherForeCast(httpClientWrapper, v2BaseUrl);
        }

        public V1WeatherForeCast WeatherForeCast { get; set; }
    }

    public class V1WeatherForeCast
    {
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly string _baseUrl;

        public V1WeatherForeCast(IHttpClientWrapper httpClientWrapper, string baseUrl)
        {
            _httpClientWrapper = httpClientWrapper;
            _baseUrl = string.Join('/', baseUrl, "WeatherForeCast");
        }

        public async Task<HttpResponseMessage> GetAsync(string city)
        {
            var url = string.Join('/', _baseUrl);
            return await _httpClientWrapper.GetAsync($"{url}?city={city}");
        }
    }

    public class V2Client
    {
        public V2Client(IHttpClientWrapper httpClientWrapper, string baseUrl)
        {
            var v2BaseUrl = string.Join('/', baseUrl, "v2");

            WeatherForeCast = new V2WeatherForeCast(httpClientWrapper, v2BaseUrl);
        }

        public V2WeatherForeCast WeatherForeCast { get; set; }
    }

    public class V2WeatherForeCast
    {
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly string _baseUrl;

        public V2WeatherForeCast(IHttpClientWrapper httpClientWrapper, string baseUrl)
        {
            _httpClientWrapper = httpClientWrapper;
            _baseUrl = string.Join('/', baseUrl, "WeatherForeCast");
        }

        public async Task<HttpResponseMessage> GetAsync(string city)
        {
            var url = string.Join('/', _baseUrl);
            return await _httpClientWrapper.GetAsync($"{url}?city={city}");
        }
    }
}