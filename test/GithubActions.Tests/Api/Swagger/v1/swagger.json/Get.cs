using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace GithubActions.Tests.Api.Swagger.v1.swagger.json
{
    public class Get : BaseClass
    {
        public Get(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        { }

        [Fact]
        public async Task should_return_200_when_healthy()
        {
            var httpResponse = await Swagger.GetJsonAsync("v1");

            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}