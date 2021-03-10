using GithubActions.Tests.Fixtures;
using Xunit.Abstractions;

namespace GithubActions.Tests.v1
{
    public class V1Fixture : TestFixture
    {
        public V1Fixture(ITestOutputHelper testOutputHelper) : base(testOutputHelper, "v1")
        {
        }
    }
}