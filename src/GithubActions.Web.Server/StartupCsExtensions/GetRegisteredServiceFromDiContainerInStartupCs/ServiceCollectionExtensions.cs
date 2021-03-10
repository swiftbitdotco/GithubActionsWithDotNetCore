using Microsoft.Extensions.DependencyInjection;

namespace GithubActions.Web.Server.StartupCsExtensions.GetRegisteredServiceFromDiContainerInStartupCs
{
    public static class ServiceCollectionExtensions
    {
        public static T GetRegisteredService<T>(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var registeredService = serviceProvider.GetService<T>();
            return registeredService;
        }
    }
}