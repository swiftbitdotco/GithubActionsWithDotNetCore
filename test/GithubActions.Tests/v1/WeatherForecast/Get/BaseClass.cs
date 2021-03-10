using GithubActions.Tests.Fixtures;
using Xunit.Abstractions;

namespace GithubActions.Tests.v1.WeatherForecast.Get
{
    public class BaseClass
    {
        protected HttpClientWrapper _client;

        protected string ResourceName = "weatherforecast";

        protected string GetRequestUri(string city)
        {
            return $"{ResourceName}?city={city}";
        }

        public BaseClass(ITestOutputHelper testOutputHelper)
        {
            _client = new V1Fixture(testOutputHelper).CreateClient();
        }
    }
}