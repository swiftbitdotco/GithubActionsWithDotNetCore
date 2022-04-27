using Microsoft.Extensions.DependencyInjection;

namespace GithubActions.Web.Server.v2.Application.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationV2(this IServiceCollection services)
        {
            services.AddTransient<ISomeService, SomeService>();
        }
    }
}