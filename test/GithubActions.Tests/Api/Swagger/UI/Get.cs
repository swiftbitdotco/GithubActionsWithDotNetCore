using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace GithubActions.Tests.Api.Swagger.UI
{
    public class Get : BaseClass
    {
        public Get(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        { }

        [Fact]
        public async Task should_return_200_when_healthy()
        {
            var httpResponse = await Swagger.GetUiAsync();

            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}