using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using GithubActions.Contract;
using GithubActions.Shared.Extensions;
using GithubActions.Tests.TestInfrastructure.Fixtures;
using GithubActions.Web.Server.v2.Application;
using GithubActions.Web.Server.v2.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;
using Xunit.Abstractions;

namespace GithubActions.Tests.Api.ReturningHttp500
{
    public class when_an_exception_occurs : IDisposable
    {
        private readonly ITestOutputHelper _logger;
        private readonly ApiTestFixture _fixture;

        public when_an_exception_occurs(ITestOutputHelper logger)
        {
            _logger = logger;
            _fixture = new ApiTestFixture(logger, new List<ServiceDescriptor>
            {
                new(typeof(ISomeService), (_) => throw new Exception("Kaboom!"), ServiceLifetime.Singleton)
            });
        }

        [Fact]
        public async Task should_return_http_500_InternalServerError()
        {
            if (!_fixture.TestSettings.IsRunningOnLocalHost)
            {
                _logger.WriteLine("This test will not work when running remotely");
                Assert.True(true);
                return;
            }

            // ARRANGE
            var client = _fixture.CreateClient();

            // ACT
            var httpResponse = await client.V2.WeatherForeCast.GetAsync("london");

            // ASSERT
            // check status code
            httpResponse.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

            // check body
            var response = await httpResponse.GetContentAsAsync<InternalServerErrorResponse>();
            response.Should().NotBeNull();

            response.DateTimeOccurredUtc.Should().NotBeNullOrWhiteSpace();
            response.Message.Should().Be("Kaboom!");
            response.Exception.Should().NotBeNullOrWhiteSpace();
        }

        public void Dispose()
        {
            _fixture?.Dispose();
        }
    }
}