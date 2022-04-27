using System;
using System.Net.Http;

namespace GithubActions.Tests.TestInfrastructure.HttpClientFactories
{
    public interface IHttpClientFactoryForTests : IDisposable
    {
        HttpClient CreateHttpClient();
    }
}