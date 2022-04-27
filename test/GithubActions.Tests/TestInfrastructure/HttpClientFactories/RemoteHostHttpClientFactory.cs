using System;
using System.Net.Http;
using GithubActions.Shared.Clients;

namespace GithubActions.Tests.TestInfrastructure.HttpClientFactories
{
    public class RemoteHostHttpClientFactory : IHttpClientFactoryForTests
    {
        private readonly Uri _baseAddress;

        public RemoteHostHttpClientFactory(Uri baseAddress)
        {
            _baseAddress = baseAddress;
        }

        public HttpClient CreateHttpClient()
        {
            return new HttpClient
            {
                BaseAddress = _baseAddress
            }.WithDefaultHeaders();
        }

        public void Dispose()
        {
        }
    }
}