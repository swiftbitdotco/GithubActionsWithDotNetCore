using System.IO;
using System.Reflection;
using GithubActions.Tests.TestInfrastructure.Fixtures;
using GithubActions.Web.Server;
using Microsoft.Extensions.Configuration;

namespace GithubActions.Tests.TestConfiguration
{
    public static class ConfigurationBuilderFactory
    {
        public static IConfigurationBuilder CreateBuilder()
        {
            var builder = new ConfigurationBuilder();

            builder.BuildForTestsProject();

            return builder;
        }
    }

    public static class ConfigBuilderExtensions
    {
        public static IConfigurationBuilder BuildForTestsProject(this IConfigurationBuilder configurationBuilder)
        {
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            configurationBuilder
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddJsonFile("testSettings.json", optional: false, reloadOnChange: false)
                .AddUserSecrets<Program>()
                .AddUserSecrets<ApiTestFixture>()
                .AddEnvironmentVariables()
                ;

            return configurationBuilder;
        }
    }
}