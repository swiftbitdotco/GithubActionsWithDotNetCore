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
    public interface ICustomHttpClient
    {
        Uri BaseAddress { get; }

        Task<HttpResponseMessage> PostAsync<T>(string requestUri, T postBody)
            where T : new();

        Task<HttpResponseMessage> GetAsync(string requestUri);

        Task<HttpResponseMessage> PutAsync<T>(string requestUri, T putBody)
            where T : new();

        Task<HttpResponseMessage> PutAsync(string requestUri);
    }

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

        private void ChangeAcceptAndContentTypeHeadersTo()
        {
            string applicationType = "application/json";
            // set "Content-Type" as JSON or XML (NOTE: dotnet can only do this on POST or PUT)
            _client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", applicationType);
            // set "Accept" as JSON or XML (dotnet can only do this on POST or PUT)
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(applicationType));
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string requestUri, T postBody) where T : new()
        {
            _testOutputHelper.WriteLine($"HTTP POST:'{_client.BaseAddress}{requestUri}'");

            var stringContent = await GetContent(postBody);

            var httpResponse = await _client.PostAsync(requestUri, stringContent);

            await PrettyPrintResponse(httpResponse);
            return httpResponse;
        }

        private async Task<StringContent> GetContent<T>(T postBody) where T : new()
        {
            ChangeAcceptAndContentTypeHeadersTo();

            var content = JsonConvert.SerializeObject(postBody, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Formatting = Formatting.Indented
            });

            var stringContent = new StringContent(content, Encoding.UTF8);

            await PrettyPrintRequest(stringContent);

            return stringContent;
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            _testOutputHelper.WriteLine($"HTTP GET:'{_client.BaseAddress}{requestUri}'");

            ChangeAcceptAndContentTypeHeadersTo();

            var httpResponse = await _client.GetAsync(requestUri);

            await PrettyPrintResponse(httpResponse);
            return httpResponse;
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string requestUri, T putBody) where T : new()
        {
            _testOutputHelper.WriteLine($"HTTP PUT:'{_client.BaseAddress}{requestUri}'");

            var stringContent = await GetContent(putBody);

            var httpResponse = await _client.PutAsync(requestUri, stringContent);

            await PrettyPrintResponse(httpResponse);
            return httpResponse;
        }

        public async Task<HttpResponseMessage> PutAsync(string requestUri)
        {
            _testOutputHelper.WriteLine($"HTTP PUT:'{_client.BaseAddress}{requestUri}'");

            var stringContent = await GetContent(new object());

            var httpResponse = await _client.PutAsync(requestUri, stringContent);

            await PrettyPrintResponse(httpResponse);
            return httpResponse;
        }

        private async Task PrettyPrintRequest(HttpContent stringContent)
        {
            await PrettyPrint("Request", stringContent);
        }

        private async Task PrettyPrintResponse(HttpResponseMessage httpResponseMessage)
        {
            await PrettyPrint("Response", httpResponseMessage.Content);
        }

        private async Task PrettyPrint(string requestOrResponse, HttpContent stringContent)
        {
            string prettyPrintRequestBody = await stringContent.TryGetPrettyPrintJson();

            _testOutputHelper.WriteLine($"{requestOrResponse} Content:'{prettyPrintRequestBody}'");
        }
    }
}