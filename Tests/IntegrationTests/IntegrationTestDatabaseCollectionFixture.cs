using Xunit;

namespace Tests.IntegrationTests
{
    [CollectionDefinition("DockerDatabaseCollection")]
    public class IntegrationTestDatabaseCollectionFixture : ICollectionFixture<IntegrationTestApplicationFactory>
    {
    }
}
