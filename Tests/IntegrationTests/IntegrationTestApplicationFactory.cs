using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Tests.IntegrationTests
{

    public class IntegrationTestApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly string _testConnectionString;
        private readonly IContainer _dbContainer = new ContainerBuilder()
                .WithImage("smwschemaonly:latest")
                .WithPortBinding(1433, 1433)
                .WithName("SqlServerTestContainer")
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
            await _dbContainer.StartAsync();
            await Task.Delay(5000);
        }
        public new async Task DisposeAsync()
        {
            await _dbContainer.StopAsync();
        }
    }
}
