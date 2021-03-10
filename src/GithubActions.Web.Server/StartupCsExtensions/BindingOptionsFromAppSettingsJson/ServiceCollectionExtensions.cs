using GithubActions.Web.Server.StartupCsExtensions.GetRegisteredServiceFromDiContainerInStartupCs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GithubActions.Web.Server.StartupCsExtensions.BindingOptionsFromAppSettingsJson
{
    public static class ServiceCollectionExtensions
    {
        public static T ConfigureAndBindFromAppSettingsJson<T>(this IServiceCollection services) where T : class, new()
        {
            var configuration = services.GetRegisteredService<IConfiguration>();

            var alreadyRegistered = services.GetRegisteredService<T>();
            if (alreadyRegistered != null)
            {
                return alreadyRegistered;
            }

            T retVal = new T();
            var appSettingsSection = configuration.GetSection(typeof(T).Name);
            appSettingsSection.Bind(retVal);
            services.Configure<T>(appSettingsSection);

            return retVal;
        }
    }
}