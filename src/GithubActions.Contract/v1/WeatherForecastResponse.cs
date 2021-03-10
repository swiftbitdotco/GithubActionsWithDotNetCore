using System.Collections.Generic;

namespace GithubActions.Contract.v1
{
    public class WeatherForecastResponse
    {
        public WeatherForecastResponse()
        {
            WeatherForecasts = new List<WeatherForecast>();
        }

        public string City { get; set; }

        public IEnumerable<WeatherForecast> WeatherForecasts { get; set; }
    }
}