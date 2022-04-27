using Microsoft.Extensions.Configuration;

namespace GithubActions.Tests.TestConfiguration
{
    public static class ConfigurationExtensions
    {
        public static T LoadSection<T>(this IConfigurationRoot configuration) where T : class, new()
        {
            var concreteSection = new T();
            var name = concreteSection.GetType().Name;

            configuration.GetSection(name).Bind(concreteSection);

            return concreteSection;
        }

        public static T LoadFromRoot<T>(this IConfigurationRoot configuration) where T : class, new()
        {
            var concreteSection = new T();

            configuration.Bind(concreteSection);

            return concreteSection;
        }
    }
}