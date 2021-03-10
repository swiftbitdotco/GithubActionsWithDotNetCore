using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace GithubActions.Tests.Fixtures
{
    public class TestFixture : IDisposable
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private string ApiVersionSegmentInUri { get; }
        private CustomWebApplicationFactory _server;

        public TestFixture(ITestOutputHelper testOutputHelper, string apiVersionSegmentInUri)
        {
            if (string.IsNullOrWhiteSpace(apiVersionSegmentInUri))
            {
                throw new TestFixtureException($"'{nameof(apiVersionSegmentInUri)}' param must not be null");
            }

            _testOutputHelper = testOutputHelper;

            ApiVersionSegmentInUri = apiVersionSegmentInUri;

            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var configRoot = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                //.AddUserSecrets<TestFixture>()
                .AddEnvironmentVariables()
                .Build();

            // load test settings
            TestSettings = LoadSection<TestSettings>(configRoot);

            // TODO: load & print app-specific configs to the console in order to see their values in the build server output
            // e.g. LoadSection<MyServiceSettings>(configRoot);
        }

        private static T LoadSection<T>(IConfiguration configuration) where T : class, new()
        {
            var concreteSection = new T();
            var name = concreteSection.GetType().Name;

            configuration.GetSection(name).Bind(concreteSection);

            return concreteSection;
        }

        private TestSettings TestSettings { get; }

        public HttpClientWrapper CreateClient()
        {
            return TestSettings.TargetLocalHost
                ? CreateHttpClientForLocalTesting()
                : CreateHttpClientForTestingAgainstARemoteApi();
        }

        private HttpClientWrapper CreateHttpClientForTestingAgainstARemoteApi()
        {
            TestSettings.BaseApiUrlFormatString.Should().NotBeNullOrWhiteSpace();
            TestSettings.BaseApiUrlFormatString.Should().Contain("{VERSION}");

            var uri = TestSettings.BaseApiUrlFormatString
                .Replace("{VERSION}", $"{ApiVersionSegmentInUri}");

            uri.Should().NotContain("{VERSION}");
            TestSettings.ApiKeyHeader.Should().NotBeNullOrWhiteSpace("ApiKeyHeader should not be null or whitespace");
            TestSettings.ApiKeyValue.Should().NotBeNullOrWhiteSpace("ApiKeyValue should not be null or whitespace");

            var client = new HttpClient
            {
                BaseAddress = new Uri(uri),
            };
            client.DefaultRequestHeaders.Add(TestSettings.ApiKeyHeader, TestSettings.ApiKeyValue);

            CheckUri(client.BaseAddress.AbsoluteUri);
            return new HttpClientWrapper(client, _testOutputHelper);
        }

        private HttpClientWrapper CreateHttpClientForLocalTesting()
        {
            _server = new CustomWebApplicationFactory(_testOutputHelper);
            var httpClientWrapper = _server.CreateHttpClientWrapper(ApiVersionSegmentInUri);
            CheckUri(httpClientWrapper.BaseAddress.AbsoluteUri);
            return httpClientWrapper;
        }

        private void CheckUri(string absoluteUri)
        {
            absoluteUri.Should().EndWith("/", "without a trailing slash, the last part of the url path is discarded when using HttpClient.");
            absoluteUri.Should().Contain($"/{ApiVersionSegmentInUri}/", "Version segment is not properly surrounded by /'s.");

            var test = absoluteUri
                .Replace("http://", null)
                .Replace("https://", null);
            test.Should().NotContain("//", "No double-slashes allowed in the url.");
        }

        public void Dispose()
        {
            _server?.Dispose();
        }
    }

    public class TestFixtureException : Exception
    {
        public TestFixtureException(string message) : base(message)
        {
        }
    }
}