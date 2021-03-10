using GithubActions.Web.Server.StartupCsExtensions.SwaggerCustomization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GithubActions.Web.Server.StartupCsExtensions.ApiVersioning
{
    public static class ServiceCollectionExtensions
    {
        public static void SetupApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(
                options =>
                {
                    options.ReportApiVersions = true;
                    // https://github.com/microsoft/aspnet-api-versioning/wiki/API-Version-Conventions#version-by-namespace-convention
                    options.Conventions.Add(new VersionByNamespaceConvention());

                    // these two lines will route the requests to v1 if no version is specified
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                });

            services.AddVersionedApiExplorer(
                options =>
                {
                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        }
    }
}