using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using GithubActions.Tests.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace GithubActions.Tests.swagger.v1.swagger.json
{
    public class Get
    {
        private readonly HttpClientWrapper _client;

        public Get(ITestOutputHelper testOutputHelper)
        {
            _client = new TestFixture(testOutputHelper, "swagger").CreateClient();
        }

        [Fact]
        public async Task should_return_200_when_healthy()
        {
            var httpResponse = await _client.GetAsync("v1/swagger.json");

            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}