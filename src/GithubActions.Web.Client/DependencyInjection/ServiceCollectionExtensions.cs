using System;
using System.Net.Http;
using System.Net.Http.Headers;
using GithubActions.Shared;
using GithubActions.Shared.Clients;
using GithubActions.Shared.Logging;
using GithubActions.Web.Client.Configuration;
using GithubActions.Web.Client.Data;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GithubActions.Web.Client.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void InjectDependencies(this WebAssemblyHostBuilder builder)
        {
            var services = builder.Services;
            var configuration = builder.Configuration;

            // default (from Program.cs)
            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            // shared
            services.AddScoped<ISimpleLogger, SimpleLogger>();
            services.AddScoped<ICustomHttpClientFactory, CustomHttpClientFactory>();
            services.AddScoped<IApiClient, ApiClient>();

            // this project
            services.AddScoped<IWeatherService, WeatherService>();

            var uris = new Uris();
            configuration.GetSection(nameof(Uris)).Bind(uris);
            //services.AddSingleton(uris);
            Console.WriteLine($"API url: {uris.Backend.AbsoluteUri}");

            services.SetupHttpClient(ApiClient.Name, uris.Backend.AbsoluteUri);
        }

        public static void SetupHttpClient(this IServiceCollection services, string clientName, string baseAddress)
        {
            if (!Uri.TryCreate(baseAddress, UriKind.Absolute, out var uri))
            {
                throw new Exception($"String '{baseAddress}' is not a valid URI");
            }

            services.AddHttpClient(clientName, client =>
            {
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
        }
    }
}