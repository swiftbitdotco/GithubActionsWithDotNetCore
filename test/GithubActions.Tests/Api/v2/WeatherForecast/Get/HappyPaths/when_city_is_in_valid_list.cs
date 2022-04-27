using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using GithubActions.Contract.v1;
using GithubActions.Shared.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace GithubActions.Tests.Api.v2.WeatherForecast.Get.HappyPaths
{
    public class when_city_is_in_valid_list : BaseClass
    {
        public when_city_is_in_valid_list(
            ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        { }

        [Theory]
        [InlineData("London")]
        [InlineData("Paris")]
        public async Task should_return_200_OK(string city)
        {
            // ARRANGE

            // ACT
            var httpResponse = await Client.WeatherForeCast.GetAsync(city);

            // ASSERT
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // check response body
            var result = await httpResponse.GetContentAsAsync<WeatherForecastResponse>();
            result.City.Should().Be(city);
        }
    }
}