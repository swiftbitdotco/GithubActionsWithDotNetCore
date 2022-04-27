using System;
using FluentAssertions;

#nullable enable

namespace GithubActions.Tests.TestConfiguration
{
    public class TestSettings
    {
        /// <summary>
        /// Use either "https://localhost/api/" or the environment url from Azure (eg. "https://biltong-service.azurewebsites.net").
        /// </summary>
        public Uri BaseUrl { get; set; } = null!;

        /// <summary>
        /// Used to spin up the application locally.
        /// </summary>
        public bool IsRunningOnLocalHost => BaseUrl == null || BaseUrl.AbsoluteUri.Contains("localhost");

        public void Validate()
        {
            BaseUrl.Should().NotBeNull();
        }
    }
}