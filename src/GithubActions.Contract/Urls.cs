using System;

namespace GithubActions.Contract
{
    public class Urls
    {
        public Urls(Uri baseUri, int? version)
        {
            var trimmedBaseUrl = baseUri.AbsoluteUri.TrimEnd('/');
            var versionString = version.HasValue ? $"v{version}" : string.Empty;

            BaseUrl = string.Join("/", trimmedBaseUrl, versionString).TrimEnd('/');
            WeatherForecast = new _WeatherForecast(BaseUrl);
        }

        private string BaseUrl { get; }
        public _WeatherForecast WeatherForecast { get; }
    }

    public class _WeatherForecast
    {
        public _WeatherForecast(string baseUrl)
        {
            BaseUrl = string.Join("/", baseUrl, "weatherforecast");
        }

        private string BaseUrl { get; }

        public string Get(string city)
        {
            return $"{BaseUrl}?city={city}";
        }
    }
}