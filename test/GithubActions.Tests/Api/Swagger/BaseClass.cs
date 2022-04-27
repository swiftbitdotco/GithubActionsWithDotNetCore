using GithubActions.Tests.TestInfrastructure.Fixtures;
using Xunit.Abstractions;

namespace GithubActions.Tests.Api.Swagger
{
    public class BaseClass
    {
        public Shared.Clients.Swagger Swagger { get; set; }

        public BaseClass(ITestOutputHelper logger)
        {
            var fixture = new ApiTestFixture(logger);
            Swagger = fixture.CreateClient().Swagger;
        }
    }
}