using System;
using System.Collections.Generic;
using GithubActions.Shared.Clients;
using GithubActions.Tests.TestConfiguration;
using GithubActions.Tests.TestInfrastructure.HttpClientFactories;
using GithubActions.Web.Server.v2.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace GithubActions.Tests.TestInfrastructure.Fixtures
{
    public class ApiTestFixture : IDisposable
    {
        private readonly ITestOutputHelper _logger;
        private readonly IHttpClientFactoryForTests _httpClientFactory;

        public ApiTestFixture(ITestOutputHelper logger, List<ServiceDescriptor> servicesToReplace = null)
        {
            _logger = logger;
            var configRoot = ConfigurationBuilderFactory.CreateBuilder().Build();
            TestSettings = configRoot.LoadSection<TestSettings>();
            TestSettings.Validate();

            logger.WriteLine($"Using {nameof(TestConfiguration.TestSettings)}:'{JsonConvert.SerializeObject(TestSettings, Formatting.Indented)}'");

            WeatherConfig = configRoot.LoadSection<WeatherConfig>();
            logger.WriteLine($"Using {nameof(Web.Server.v2.Configuration.WeatherConfig)}:'{JsonConvert.SerializeObject(WeatherConfig, Formatting.Indented)}'");

            if (TestSettings.IsRunningOnLocalHost)
            {
                _httpClientFactory = new LocalHostHttpClientFactory(logger, servicesToReplace);
            }
            else
            {
                logger.WriteLine($"# RUNNING TESTS AGAINST:'{TestSettings.BaseUrl}'");
                _httpClientFactory = new RemoteHostHttpClientFactory(TestSettings.BaseUrl);
            }
        }

        public TestSettings TestSettings { get; set; }
        public WeatherConfig WeatherConfig { get; set; }

        public ApiClient CreateClient()
        {
            var httpClient = _httpClientFactory.CreateHttpClient();
            var client = HttpClientWrapperFactoryForTests
                .CreateHttpClientWrapper(httpClient, _logger);
            return new ApiClient(client);
        }

        public void Dispose()
        {
            _httpClientFactory?.Dispose();
        }
    }
}