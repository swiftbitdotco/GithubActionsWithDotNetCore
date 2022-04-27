using GithubActions.Tests.Extensions;
using GithubActions.Tests.TestInfrastructure.Fixtures;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace GithubActions.Tests
{
    public class when_using_the_TestFixture
    {
        private readonly ITestOutputHelper _logger;
        private readonly ApiTestFixture _fixture;

        public when_using_the_TestFixture(ITestOutputHelper logger)
        {
            _logger = logger;
            _fixture = new ApiTestFixture(_logger);
        }

        [Fact]
        public void should_be_able_to_resolve_all_properties_of_application_settings()
        {
            var weatherConfig = _fixture.WeatherConfig;
            _logger.WriteLine($"{weatherConfig.GetType().Name}: {JsonConvert.SerializeObject(weatherConfig)}");
            weatherConfig.ShouldNotHaveAnyNullProperties();
        }

        [Fact]
        public void should_be_able_to_resolve_all_properties_of_test_settings()
        {
            var testSettings = _fixture.TestSettings;
            _logger.WriteLine($"{_fixture.TestSettings.GetType().Name}: {JsonConvert.SerializeObject(testSettings)}");
            _fixture.TestSettings.ShouldNotHaveAnyNullProperties();
        }
    }
}