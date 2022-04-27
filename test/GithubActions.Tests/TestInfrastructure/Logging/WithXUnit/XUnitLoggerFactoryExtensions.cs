using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace GithubActions.Tests.TestInfrastructure.Logging.WithXUnit
{
    public static class XUnitLoggerFactoryExtensions
    {
        public static ILoggingBuilder AddXUnit(this ILoggingBuilder builder, ITestOutputHelper logger)
        {
            builder.Services.AddSingleton(logger);
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, XUnitLoggerProvider>());

            return builder;
        }
    }
}