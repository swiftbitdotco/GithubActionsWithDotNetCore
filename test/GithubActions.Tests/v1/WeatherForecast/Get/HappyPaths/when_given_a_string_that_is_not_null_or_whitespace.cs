using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using GithubActions.Contract.v1;
using GithubActions.Shared.Extensions;
using GithubActions.Tests.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace GithubActions.Tests.v1.WeatherForecast.Get.HappyPaths
{
    public class when_given_a_string_that_is_not_null_or_whitespace : BaseClass
    {
        public when_given_a_string_that_is_not_null_or_whitespace(ITestOutputHelper testOutputHelper) : base(
            testOutputHelper)
        { }

        [Theory]
        [InlineData("London")]
        [InlineData("Paris")]
        public async Task should_return_200_OK(string city)
        {
            // ARRANGE
            var uri = GetRequestUri(city);

            // ACT
            var httpResponse = await _client.GetAsync(uri);

            // ASSERT
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // check response body
            var result = await httpResponse.GetContentAsAsync<WeatherForecastResponse>();
            result.City.Should().Be(city);
        }
    }
}