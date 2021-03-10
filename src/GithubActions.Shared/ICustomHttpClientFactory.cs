using System.Net.Http;
using GithubActions.Shared.Logging;

namespace GithubActions.Shared
{
    public interface ICustomHttpClientFactory
    {
        ICustomHttpClient CreateClient(HttpClient httpClient);
    }

    public class CustomHttpClientFactory : ICustomHttpClientFactory
    {
        private readonly ISimpleLogger _logger;

        public CustomHttpClientFactory(ISimpleLogger logger)
        {
            _logger = logger;
        }

        public ICustomHttpClient CreateClient(HttpClient httpClient)
        {
            return new CustomHttpClient(httpClient, _logger);
        }
    }
}