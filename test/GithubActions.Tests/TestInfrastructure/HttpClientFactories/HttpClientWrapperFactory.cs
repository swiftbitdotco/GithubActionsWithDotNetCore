using System;
using System.Net.Http;
using GithubActions.Shared.Clients;
using Xunit.Abstractions;

namespace GithubActions.Tests.TestInfrastructure.HttpClientFactories
{
    public class HttpClientWrapperFactoryForTests
    {
        public class HttpClientWrapperFacade : IHttpClientWrapperLogger
        {
            private readonly ITestOutputHelper _logger;

            public HttpClientWrapperFacade(ITestOutputHelper logger)
            {
                _logger = logger;
            }

            public void WriteLine(string message)
            {
                _logger.WriteLine(message);
            }
        }

        public static IHttpClientWrapper CreateHttpClientWrapper(HttpClient httpClient, ITestOutputHelper logger)
        {
            var baseAddress = httpClient.BaseAddress.AbsoluteUri;

            if (!baseAddress.EndsWith('/'))
            {
                baseAddress += '/';
            }

            httpClient.BaseAddress = new Uri(baseAddress);

            return new HttpClientWrapper(httpClient, new HttpClientWrapperFacade(logger));
        }
    }
}