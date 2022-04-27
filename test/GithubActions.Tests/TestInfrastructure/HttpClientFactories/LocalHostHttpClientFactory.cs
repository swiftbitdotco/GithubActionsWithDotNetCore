using System.Collections.Generic;
using System.Net.Http;
using GithubActions.Shared.Clients;
using GithubActions.Tests.TestConfiguration;
using GithubActions.Tests.TestInfrastructure.Logging.WithXUnit;
using GithubActions.Web.Server;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace GithubActions.Tests.TestInfrastructure.HttpClientFactories
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0
    /// </summary>
    public class LocalHostHttpClientFactory : WebApplicationFactory<Program>, IHttpClientFactoryForTests
    {
        private readonly ITestOutputHelper _logger;
        private readonly List<ServiceDescriptor> _replacements;

        public LocalHostHttpClientFactory(ITestOutputHelper logger, List<ServiceDescriptor> replacements = null)
        {
            _logger = logger;
            _replacements = replacements ?? new List<ServiceDescriptor>();
        }

        protected override IHostBuilder CreateHostBuilder()
        {
            var builder = base.CreateHostBuilder();
            builder.ConfigureAppConfiguration(configurationBuilder =>
            {
                configurationBuilder.BuildForTestsProject();
            });
            builder.ConfigureServices(services =>
            {
                foreach (var replacement in _replacements)
                {
                    services.Replace(replacement);
                }
            });
            builder.ConfigureLogging(logging =>
            {
                // only log to test console
                logging.ClearProviders();
                logging.AddXUnit(_logger);
            });

            return builder;
        }

        public HttpClient CreateHttpClient()
        {
            /*
                NOTE:
                TestServer does not open any real sockets, so running on different urls is a fictitious concept.

                You can set BaseAddress to anything you want.

                The only way to communicate with the TestServer is through the CreateClient API which creates an in-memory channel.
            */

            var httpClient = CreateClient();
            return httpClient.WithDefaultHeaders();
        }
    }
}