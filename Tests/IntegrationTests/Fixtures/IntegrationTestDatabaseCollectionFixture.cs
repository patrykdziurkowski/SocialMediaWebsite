using Xunit;

namespace Tests.IntegrationTests.Fixtures
{
    [CollectionDefinition("DockerDatabaseCollection")]
    public class IntegrationTestDatabaseCollectionFixture : ICollectionFixture<IntegrationTestApplicationFactory>
    {
    }
}
