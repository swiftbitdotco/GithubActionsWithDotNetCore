using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using GithubActions.Web.Server.StartupCsExtensions.ApiVersioning;
using GithubActions.Web.Server.StartupCsExtensions.ErrorHandling.Middleware;
using GithubActions.Web.Server.StartupCsExtensions.SwaggerCustomization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Some.Contracts;

namespace GithubActions.Web.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // important for config loading
            services.AddOptions();

            // Add the IConfiguration here so that it is available for the extension methods that use services.ConfigureAndBindFromAppSettingsJson<T>()
            services.AddSingleton(Configuration);

            services.SetupApiVersioning();

            services.SetupSwagger();

            services.AddControllers(options =>
            {
                // add a global filter for returning strongly-typed HTTP 500 responses (see ErrorHandlingMiddleware)
                options.Filters.Add(
                    new ProducesResponseTypeAttribute(typeof(InternalServerErrorResponse),
                        StatusCodes.Status500InternalServerError));

                // always respect the browser ACCEPT headers
                options.RespectBrowserAcceptHeader = true;

                // limit the request & response types to JSON
                options.Filters.Add(
                    new ProducesAttribute(
                        "application/json",
                        "multipart/form-data"));
            }).AddControllersAsServices();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
                app.SetupSwagger(provider);
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();
            //---------------------------------//
            // NOTE: For most apps, calls to UseAuthentication, UseAuthorization, and UseCors
            // must appear between the calls to UseRouting and UseEndpoints to be effective.
            // https://docs.microsoft.com/en-us/aspnet/core/migration/22-to-30?view=aspnetcore-3.1&tabs=visual-studio#migrate-startupconfigure

            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());

            app.UseAuthentication();
            //app.UseAuthorization(); <-- TODO: Azure AD tokens

            //---------------------------------//
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}