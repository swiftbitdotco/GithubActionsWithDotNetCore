using System;
using GithubActions.Web.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace GithubActions.Tests.Fixtures
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public CustomWebApplicationFactory(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Register the NUnit logger
            builder.ConfigureLogging(loggingBuilder =>
            {
                loggingBuilder.Services.AddSingleton<ILoggerProvider>(serviceProvider => new XUnitLoggerProvider(_testOutputHelper));
            });
        }

        public HttpClientWrapper CreateHttpClientWrapper(string apiVersionSegment)
        {
            var client = CreateClient();

            // append the version segment to the end of the base url
            // all new routes should have the [version] before the [controller] action
            // - i.e. http://localhost/v1/vin, http://localhost/v2/vin, etc
            var newBaseAddress = client.BaseAddress.AbsoluteUri.Trim('/');
            var uri = $"{newBaseAddress}/{apiVersionSegment}/";
            client.BaseAddress = new Uri(uri);
            return new HttpClientWrapper(client, _testOutputHelper);
        }
    }
}