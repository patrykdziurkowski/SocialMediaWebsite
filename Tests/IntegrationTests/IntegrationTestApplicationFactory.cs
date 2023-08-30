using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Tests.IntegrationTests
{

    public class IntegrationTestApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly string _testConnectionString;
        private readonly IContainerService _dbContainer = new Builder()
                .UseContainer()
                .DeleteIfExists(force: true)
                .UseImage("smwschemaonly:latest")
                .WithName("SmwSqlServer")
                .WaitForHealthy(TimeSpan.FromSeconds(10))
                .ExposePort(1433, 1433)
                .Build();

        public IntegrationTestApplicationFactory()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<IntegrationTestApplicationFactory>()
                .Build();

            _testConnectionString = configuration["ConnectionString"]
                ?? throw new ApplicationException("Connection string for the test docker container was not found in usersecrets");

        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((configBuilder) =>
            {
                configBuilder.AddInMemoryCollection(
                    new Dictionary<string, string?>
                    {
                        ["ConnectionStringName"] = "DockerSample",
                        ["ConnectionStrings:DockerSample"] = _testConnectionString
                    });
            });
        }

        public async Task InitializeAsync()
        {
            await Task.Run(() =>
            {
                _dbContainer.Start();
            });
        }
        public new async Task DisposeAsync()
        {
            await Task.Run(() =>
            {
                _dbContainer.Dispose();
            });
        }
    }
}
