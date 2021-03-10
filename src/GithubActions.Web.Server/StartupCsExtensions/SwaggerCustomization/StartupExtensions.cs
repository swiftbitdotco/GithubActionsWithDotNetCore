﻿using System;
using System.IO;
using System.Reflection;
using GithubActions.Web.Server.StartupCsExtensions.SwaggerCustomization.DocumentFilters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GithubActions.Web.Server.StartupCsExtensions.SwaggerCustomization
{
    public static class StartupExtensions
    {
        public static void SetupSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(
                options =>
                {
                    // add a custom operation filter which sets default values
                    options.OperationFilter<SwaggerDefaultValues>();

                    // Locate the XML file being generated by ASP.NET...
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                    // ... and tell Swagger to use those XML comments.
                    //options.IncludeXmlComments(xmlPath);

                    options.DocumentFilter<FillDefaultValueForApiVersionInSwaggerUi>();
                    //options.DocumentFilter<AddCallingSystemHeaderToDocument>();
                });
        }

        public static void SetupSwagger(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    // build a swagger endpoint for each discovered API version
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });
        }
    }
}