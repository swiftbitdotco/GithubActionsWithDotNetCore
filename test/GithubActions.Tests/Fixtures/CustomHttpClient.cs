using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GithubActions.Shared;
using GithubActions.Tests.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit.Abstractions;

namespace GithubActions.Tests.Fixtures
{
    public class CustomHttpClient : ICustomHttpClient
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _testOutputHelper;

        public CustomHttpClient(HttpClient client, ITestOutputHelper testOutputHelper)
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

            var stringContent = await GetContent(postBody, contentType);

            var httpResponse = await _client.PostAsync(requestUri, stringContent);

            await PrettyPrintResponse(httpResponse, contentType);
            return httpResponse;
        }

        private async Task<StringContent> GetContent<T>(T postBody, string contentType) where T : new()
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

            await PrettyPrintRequest(stringContent, contentType);

            return stringContent;
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri, string contentType = "application/json")
        {
            _testOutputHelper.WriteLine($"HTTP GET:'{_client.BaseAddress}{requestUri}'");

            ChangeAcceptAndContentTypeHeadersTo(contentType);

            var httpResponse = await _client.GetAsync(requestUri);

            await PrettyPrintResponse(httpResponse, contentType);
            return httpResponse;
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string requestUri, T putBody, string contentType = "application/json") where T : new()
        {
            _testOutputHelper.WriteLine($"HTTP PUT:'{_client.BaseAddress}{requestUri}'");

            var stringContent = await GetContent(putBody, contentType);

            var httpResponse = await _client.PutAsync(requestUri, stringContent);

            await PrettyPrintResponse(httpResponse, contentType);
            return httpResponse;
        }

        public async Task<HttpResponseMessage> PutAsync(string requestUri, string contentType = "application/json")
        {
            _testOutputHelper.WriteLine($"HTTP PUT:'{_client.BaseAddress}{requestUri}'");

            var stringContent = await GetContent(new object(), contentType);

            var httpResponse = await _client.PutAsync(requestUri, stringContent);

            await PrettyPrintResponse(httpResponse, contentType);
            return httpResponse;
        }

        private async Task PrettyPrintRequest(HttpContent stringContent, string contentType)
        {
            await PrettyPrint("Request", stringContent, contentType);
        }

        private async Task PrettyPrintResponse(HttpResponseMessage httpResponseMessage, string contentType)
        {
            await PrettyPrint("Response", httpResponseMessage.Content, contentType);
        }

        private async Task PrettyPrint(string requestOrResponse, HttpContent stringContent, string contentType = "application/json")
        {
            string prettyPrintRequestBody = await stringContent.TryGetPrettyPrintJson();

            _testOutputHelper.WriteLine($"{requestOrResponse} Content:'{prettyPrintRequestBody}'");
        }
    }
}