using FluentAssertions;
using Xunit;
using Xunit.Priority;

namespace Tests.E2ETests
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class AuthenticationTests : IClassFixture<WebServerHostService>
    {
        private readonly HttpClient _client;
        private readonly WebServerHostService _hostService;

        public AuthenticationTests(WebServerHostService hostService)
        {
            _client = new HttpClient();
            _hostService = hostService;
        }

        [Fact, Priority(0)]
        public void BothServicesRun()
        {
            //Arrange

            //Act
            bool areBothServicesRunning = _hostService.Host.Containers.All(c => c.State == Ductus.FluentDocker.Services.ServiceRunningState.Running);

            //Assert
            areBothServicesRunning.Should().BeTrue();
        }

        [Fact, Priority(5)]
        public async Task Register_GivenValidInput_Returns201()
        {
            //Arrange   
            FormUrlEncodedContent form = new(new[]
            {
                new KeyValuePair<string, string>("UserName", "JohnSmith123"),
                new KeyValuePair<string, string>("Email", "john@smith.com"),
                new KeyValuePair<string, string>("Password", "P@ssword1!"),
            });
            string uri = "http://localhost:8080/Authentication/Register";

            //Act
            HttpResponseMessage response = await _client.PostAsync(uri, form);

            //Assert
            ((int) response.StatusCode).Should().Be(201);
        }

        [Fact, Priority(10)]
        public async Task Login_GivenValidInput_Returns200()
        {
            //Arrange   
            FormUrlEncodedContent form = new(new[]
            {
                new KeyValuePair<string, string>("UserName", "JohnSmith123"),
                new KeyValuePair<string, string>("Password", "P@ssword1!"),
            });
            string uri = "http://localhost:8080/Authentication/Login";

            //Act
            HttpResponseMessage response = await _client.PostAsync(uri, form);

            //Assert
            ((int) response.StatusCode).Should().Be(200);
        }


    }
}
