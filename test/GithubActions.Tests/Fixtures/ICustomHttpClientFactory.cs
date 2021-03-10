using System;
using System.Net.Http;
using GithubActions.Shared;
using Xunit.Abstractions;

namespace GithubActions.Tests.Fixtures
{
    public interface ICustomHttpClientFactory
    {
        ICustomHttpClient CreateHttpsClientAsync(HttpClient httpClient, double? version);
    }

    public class CustomHttpClientFactory : ICustomHttpClientFactory
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public CustomHttpClientFactory(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public ICustomHttpClient CreateHttpsClientAsync(HttpClient httpClient, double? version)
        {
            var baseAddress = httpClient.BaseAddress.AbsoluteUri;

            if (!baseAddress.EndsWith('/'))
            {
                baseAddress += '/';
            }

            if (version.HasValue)
            {
                baseAddress += $"v{version}/";
            }

            httpClient.BaseAddress = new Uri(baseAddress);

            return new CustomHttpClient(httpClient, _testOutputHelper);
        }
    }
}