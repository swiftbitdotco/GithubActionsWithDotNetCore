using GithubActions.Shared.Clients;
using GithubActions.Tests.TestInfrastructure.Fixtures;
using Xunit.Abstractions;

namespace GithubActions.Tests.Api.v2
{
    public class BaseClass
    {
        public V2Client Client { get; set; }

        public BaseClass(ITestOutputHelper logger)
        {
            var fixture = new ApiTestFixture(logger);
            Client = fixture.CreateClient().V2;
        }
    }
}