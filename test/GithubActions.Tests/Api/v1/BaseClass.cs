using GithubActions.Shared.Clients;
using GithubActions.Tests.TestConfiguration;
using GithubActions.Tests.TestInfrastructure.Fixtures;
using Xunit.Abstractions;

namespace GithubActions.Tests.Api.v1
{
    public class BaseClass
    {
        public TestSettings TestSettings { get; set; }
        public V1Client Client { get; set; }

        public BaseClass(ITestOutputHelper logger)
        {
            var fixture = new ApiTestFixture(logger);
            Client = fixture.CreateClient().V1;
            TestSettings = fixture.TestSettings;
        }
    }
}