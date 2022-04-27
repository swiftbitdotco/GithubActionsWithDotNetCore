using System.Net.Http;

namespace GithubActions.Shared.Clients
{
    public static class HttpClientExtensions
    {
        public static HttpClient WithDefaultHeaders(this HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.28.2");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

            return httpClient;
        }
    }
}