using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace GithubActions.Tests.Api.v2.WeatherForecast.Get.UnhappyPaths
{
    public class when_city_is_null_or_whitespace : BaseClass
    {
        public when_city_is_null_or_whitespace(
            ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        { }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task should_return_400_BadRequest(string city)
        {
            // ARRANGE

            // ACT
            var httpResponse = await Client.WeatherForeCast.GetAsync(city);

            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}