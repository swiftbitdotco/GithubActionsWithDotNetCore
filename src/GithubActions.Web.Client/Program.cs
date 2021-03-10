using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Threading.Tasks;
using GithubActions.Web.Client.DependencyInjection;

namespace GithubActions.Web.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.InjectDependencies();

            await builder.Build().RunAsync();
        }
    }
}