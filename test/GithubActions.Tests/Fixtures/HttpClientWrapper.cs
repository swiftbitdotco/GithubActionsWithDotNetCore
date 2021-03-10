using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit.Abstractions;

namespace GithubActions.Tests.Fixtures
{
    public class HttpClientWrapper
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _testOutputHelper;

        public HttpClientWrapper(HttpClient client, ITestOutputHelper testOutputHelper)
        {
            _client = client;
            _testOutputHelper = testOutputHelper;
        }

        public Uri BaseAddress => _client.BaseAddress;

        private void ChangeAcceptAndContentTypeHeadersTo(string applicationType)
        {
            // set "Content-Type" as JSON or XML (NOTE: dotnet can only do this on POST or PUT)
            _client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", applicationType);
            // set "Accept" as JSON or XML (dotnet can only do this on POST or PUT)
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(applicationType));
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string requestUri, T postBody, string contentType = "application/json") where T : new()
        {
            _testOutputHelper.WriteLine($"HTTP POST:'{_client.BaseAddress}{requestUri}'");

            var stringContent = await GetContentAsync(postBody, contentType);

            var httpResponse = await _client.PostAsync(requestUri, stringContent);

            await PrettyPrintResponseAsync(httpResponse, contentType);
            return httpResponse;
        }

        private async Task<StringContent> GetContentAsync<T>(T postBody, string contentType) where T : new()
        {
            ChangeAcceptAndContentTypeHeadersTo(contentType);

            var content = JsonConvert.SerializeObject(postBody, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Formatting = Formatting.Indented
            });

            var stringContent = new StringContent(content, Encoding.UTF8, contentType);

            await PrettyPrintRequestAsync(stringContent, contentType);

            return stringContent;
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri, string contentType = "application/json")
        {
            _testOutputHelper.WriteLine($"HTTP GET:'{_client.BaseAddress}{requestUri}'");

            ChangeAcceptAndContentTypeHeadersTo(contentType);

            var httpResponse = await _client.GetAsync(requestUri);

            await PrettyPrintResponseAsync(httpResponse, contentType);
            return httpResponse;
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string requestUri, T putBody, string contentType = "application/json") where T : new()
        {
            _testOutputHelper.WriteLine($"HTTP PUT:'{_client.BaseAddress}{requestUri}'");

            var stringContent = await GetContentAsync(putBody, contentType);

            var httpResponse = await _client.PutAsync(requestUri, stringContent);

            await PrettyPrintResponseAsync(httpResponse, contentType);
            return httpResponse;
        }

        public async Task<HttpResponseMessage> PutAsync(string requestUri, string contentType = "application/json")
        {
            _testOutputHelper.WriteLine($"HTTP PUT:'{_client.BaseAddress}{requestUri}'");

            var stringContent = await GetContentAsync(new object(), contentType);

            var httpResponse = await _client.PutAsync(requestUri, stringContent);

            await PrettyPrintResponseAsync(httpResponse, contentType);
            return httpResponse;
        }

        private async Task PrettyPrintRequestAsync(HttpContent stringContent, string contentType)
        {
            await PrettyPrintAsync("Request", stringContent, contentType);
        }

        private async Task PrettyPrintResponseAsync(HttpResponseMessage httpResponseMessage, string contentType)
        {
            await PrettyPrintAsync("Response", httpResponseMessage.Content, contentType);
        }

        private async Task PrettyPrintAsync(string requestOrResponse, HttpContent stringContent, string contentType = "application/json")
        {
            string prettyPrintRequestBody = await stringContent.TryGetPrettyPrintContentAsync(contentType);

            _testOutputHelper.WriteLine($"{requestOrResponse} Content:'{prettyPrintRequestBody}'");
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}