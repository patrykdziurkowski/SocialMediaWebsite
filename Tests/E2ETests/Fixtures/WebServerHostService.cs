using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Xunit;

namespace Tests.E2ETests.Fixtures
{
    public class WebServerHostService : IAsyncLifetime
    {
        public readonly ICompositeService Host = new Builder()
            .UseContainer()
            .UseCompose()
            .FromFile("../../docker-compose.yaml")
            .ForceRecreate()
            .WaitForHttp("web", "http://localhost:8080/health")
            .WaitForPort("database", "14331/tcp", TimeSpan.FromSeconds(20).Seconds, "127.0.0.1")
            .Build();

        public async Task DisposeAsync()
        {
            await Task.Run(() =>
            {
                Host.Dispose();
            });
        }

        public async Task InitializeAsync()
        {
            Host.Start();
            await Task.Delay(TimeSpan.FromSeconds(5));
        }
    }
}
