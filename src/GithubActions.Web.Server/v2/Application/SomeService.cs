using System.Collections.Generic;
using GithubActions.Web.Server.v2.Configuration;
using Microsoft.Extensions.Options;

namespace GithubActions.Web.Server.v2.Application
{
    public interface ISomeService
    {
        List<string> GetSummaries();
    }

    public class SomeService : ISomeService
    {
        private readonly IOptions<WeatherConfig> _optionsWeatherConfig;

        public SomeService(IOptions<WeatherConfig> optionsWeatherConfig)
        {
            _optionsWeatherConfig = optionsWeatherConfig;
        }

        public List<string> GetSummaries()
        {
            return _optionsWeatherConfig.Value.Values;
        }
    }
}