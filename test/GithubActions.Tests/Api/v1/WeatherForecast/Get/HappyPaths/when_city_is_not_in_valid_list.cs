using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using GithubActions.Contract.v1;
using GithubActions.Shared.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace GithubActions.Tests.Api.v1.WeatherForecast.Get.HappyPaths
{
    public class when_city_is_not_in_valid_list : BaseClass
    {
        public when_city_is_not_in_valid_list(
            ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        { }

        [Theory]
        [InlineData("asdf")]
        public async Task should_return_404_not_found(string city)
        {
            // ARRANGE

            // ACT
            var httpResponse = await Client.WeatherForeCast.GetAsync(city);

            // ASSERT
            httpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}