using System;
using System.Globalization;
using System.Threading.Tasks;
using GithubActions.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GithubActions.Web.Server.StartupCsExtensions.ErrorHandling.Middleware
{
    /// <summary>
    /// NOTE: must be registered before "<c>app.UseMvc();</c>" or "<c>app.UseControllers();</c>"
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        // NOTE: This middleware must be registered before "<c>app.UseMvc();</c>"
        // NOTE: Code taken from Approach #2 in https://stackoverflow.com/a/38935583

        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        //private readonly TelemetryClient _telemetryClient;

        //public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, TelemetryClient telemetryClient)
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            //_telemetryClient = telemetryClient;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                // Report the exception to Application Insights.
                // _telemetryClient.TrackException(ex);

                //await HandleExceptionAsync(context, ex, _telemetryClient);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // NOTE: Here we can assign specific return codes for specific types of Exception...
            // e.g. if exception is DomainException, do something with it...

            // 500 if unexpected
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = new InternalServerErrorResponse
            {
                DateTimeOccurredUtc = DateTime.UtcNow.ToString("u", CultureInfo.InvariantCulture),
                Message = ex.Message,
                Exception = ex.ToString()
            };

            var responseStringContent = JsonConvert.SerializeObject(response);
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(responseStringContent);
        }
    }
}