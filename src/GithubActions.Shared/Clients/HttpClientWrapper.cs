using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GithubActions.Shared.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GithubActions.Shared.Clients
{
    public interface IHttpClientWrapper : IDisposable
    {
        Task<HttpResponseMessage> PostAsync<T>(string requestUri, T postBody, string token = null)
            where T : new();

        Task<HttpResponseMessage> GetAsync(string requestUri);

        Task<HttpResponseMessage> PutAsync<T>(string requestUri, T putBody);

        Task<HttpResponseMessage> PutAsync(string requestUri);

        Task<HttpResponseMessage> DeleteAsync(string requestUri);
    }

    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly string ContentType = "application/json";
        private readonly HttpClient _client;
        private readonly IHttpClientWrapperLogger _logger;

        public HttpClientWrapper(HttpClient client, IHttpClientWrapperLogger logger)
        {
            _client = client;
            _logger = logger;
        }

        public Uri BaseAddress => _client.BaseAddress;

        public async Task<HttpResponseMessage> PostAsync<T>(string requestUri, T postBody, string token = null) where T : new()
        {
            _logger.WriteLine($"HTTP POST:'{_client.BaseAddress}{requestUri}'");

            var httpContent = ToStringContent(postBody);

            var httpRequestMessage = CreateNewHttpRequest(httpContent, HttpMethod.Post, requestUri, token);

            var httpResponseMessage = await _client.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead);

            PrettyPrintResponse(httpResponseMessage);
            return httpResponseMessage;
        }

        private StringContent ToStringContent<T>(T postBody)
        {
            var content = JsonConvert.SerializeObject(postBody, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Formatting = Formatting.Indented
            });

            return new StringContent(content, Encoding.UTF8, ContentType);
        }

        private HttpRequestMessage CreateNewHttpRequest(HttpContent httpContent, HttpMethod httpMethod, string requestUri, string token = null)
        {
            var request = new HttpRequestMessage(httpMethod, requestUri)
            {
                Content = httpContent
            };

            request.Content.Headers.ContentType = new MediaTypeHeaderValue(ContentType);
            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");
            }

            PrettyPrintRequest(request);
            return request;
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            _logger.WriteLine($"HTTP GET:'{_client.BaseAddress}{requestUri}'");

            var httpResponse = await _client.GetAsync(requestUri);

            PrettyPrintResponse(httpResponse);
            return httpResponse;
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string requestUri, T putBody)
        {
            _logger.WriteLine($"HTTP PUT:'{_client.BaseAddress}{requestUri}'");

            var httpContent = ToStringContent(putBody);
            var httpRequest = CreateNewHttpRequest(httpContent, HttpMethod.Put, requestUri);

            var httpResponse = await _client.SendAsync(httpRequest);

            PrettyPrintResponse(httpResponse);
            return httpResponse;
        }

        public async Task<HttpResponseMessage> PutAsync(string requestUri)
        {
            _logger.WriteLine($"HTTP PUT:'{_client.BaseAddress}{requestUri}'");

            var httpContent = ToStringContent(new object());
            var httpRequest = CreateNewHttpRequest(httpContent, HttpMethod.Put, requestUri);

            var httpResponse = await _client.SendAsync(httpRequest);

            PrettyPrintResponse(httpResponse);
            return httpResponse;
        }

        public async Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            _logger.WriteLine($"HTTP DELETE:'{_client.BaseAddress}{requestUri}'");

            var httpResponse = await _client.DeleteAsync(requestUri);

            PrettyPrintResponse(httpResponse);
            return httpResponse;
        }

        private void PrettyPrintRequest(HttpRequestMessage httpRequestMessage)
        {
            _logger.WriteLine("----> Request");
            _logger.WriteLine($"{httpRequestMessage}");
            try
            {
                var content = httpRequestMessage.Content.TryGetPrettyPrintJson().GetAwaiter().GetResult();
                _logger.WriteLine($"Content: {content}");
            }
            catch
            {
                // carry on, we don't really care
            }
            _logger.WriteLine("<------------");
        }

        private void PrettyPrintResponse(HttpResponseMessage httpResponseMessage)
        {
            _logger.WriteLine("----> Response");
            _logger.WriteLine($"{httpResponseMessage}");
            try
            {
                var content = httpResponseMessage.Content.TryGetPrettyPrintJson().GetAwaiter().GetResult();
                _logger.WriteLine($"Content: {content}");
            }
            catch
            {
                // carry on, we don't really care
            }
            _logger.WriteLine("<------------");
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}