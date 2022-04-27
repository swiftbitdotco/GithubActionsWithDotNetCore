//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.DependencyInjection.Extensions;
//using Microsoft.Extensions.Logging;

//namespace GithubActions.Tests.TestInfrastructure.Logging.WithNUnit
//{
//    public static class NUnitLoggerFactoryExtensions
//    {
//        public static ILoggingBuilder AddNUnit(this ILoggingBuilder builder)
//        {
//            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, NUnitLoggerProvider>());

//            return builder;
//        }
//    }
//}