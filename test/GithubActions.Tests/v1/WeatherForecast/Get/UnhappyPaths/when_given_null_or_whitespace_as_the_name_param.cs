using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace GithubActions.Tests.v1.WeatherForecast.Get.UnhappyPaths
{
    public class when_given_null_or_whitespace_as_the_city_param : BaseClass
    {
        public when_given_null_or_whitespace_as_the_city_param(ITestOutputHelper testOutputHelper) : base(
            testOutputHelper)
        { }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task should_return_400_BadRequest(string city)
        {
            // ARRANGE
            var uri = GetRequestUri(city);

            // ACT
            var httpResponse = await _client.GetAsync(uri);

            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}