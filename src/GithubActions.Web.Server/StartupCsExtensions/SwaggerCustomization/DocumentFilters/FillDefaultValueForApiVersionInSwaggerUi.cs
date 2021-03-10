using System.Linq;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GithubActions.Web.Server.StartupCsExtensions.SwaggerCustomization.DocumentFilters
{
    // Adapted from https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/346
    public class FillDefaultValueForApiVersionInSwaggerUi : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var version = swaggerDoc.Info.Version;

            foreach (var pathItem in swaggerDoc.Paths.Values)
            {
                foreach (var operation in pathItem.Operations.Values)
                {
                    TryAddVersionParamTo(operation, version);
                }
            }
        }

        private void TryAddVersionParamTo(OpenApiOperation operation, string version)
        {
            var apiVersionParameter = operation?.Parameters?.FirstOrDefault(x => x.Name == "api-version");
            if (apiVersionParameter == null)
            {
                return;
            }

            apiVersionParameter.Schema.Default = new OpenApiString(version);
        }
    }
}