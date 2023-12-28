using FluentAssertions;
using Tests.E2ETests.Fixtures;
using Xunit;
using Xunit.Priority;

namespace Tests.E2ETests
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    [Collection("WebServerTests")]
    public class AuthenticationTests : IClassFixture<WebServerHostService>
    {
        private static readonly HttpClient _client = new(new HttpClientHandler() { CookieContainer = new System.Net.CookieContainer() });

        private readonly WebServerHostService _hostService;

        public AuthenticationTests(WebServerHostService hostService)
        {
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

        [Fact, Priority(3)]
        public async Task UnauthorizedAccess_Returns405()
        {
            //Arrange   
            string uri = "http://localhost:8080/Conversations";

            //Act
            HttpResponseMessage response = await _client.GetAsync(uri);

            //Assert
            ((int) response.StatusCode).Should().Be(405);
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

        [Fact, Priority(7)]
        public async Task Register_GivenDuplicateUserName_Returns403()
        {
            //Arrange   
            FormUrlEncodedContent form = new(new[]
            {
                new KeyValuePair<string, string>("UserName", "JohnSmith123"),
                new KeyValuePair<string, string>("Email", "john@notsmith.com"),
                new KeyValuePair<string, string>("Password", "P@ssword1!1"),
            });
            string uri = "http://localhost:8080/Authentication/Register";

            //Act
            HttpResponseMessage response = await _client.PostAsync(uri, form);

            //Assert
            ((int) response.StatusCode).Should().Be(403);
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

        [Fact, Priority(15)]
        public async Task AuthorizedAccess_Returns200()
        {
            //Arrange   
            string uri = "http://localhost:8080/Conversations";

            //Act
            HttpResponseMessage response = await _client.GetAsync(uri);

            //Assert
            ((int) response.StatusCode).Should().Be(200);
        }

        [Fact, Priority(20)]
        public async Task Logout_Returns200()
        {
            //Arrange   
            string uri = "http://localhost:8080/Authentication/Logout";

            //Act
            HttpResponseMessage response = await _client.PostAsync(uri, null);

            //Assert
            ((int) response.StatusCode).Should().Be(200);
        }

        [Fact, Priority(25)]
        public async Task AccessingUnauthorizedEndpoint_AfterLogout_Returns405()
        {
            //Arrange   
            string uri = "http://localhost:8080/Conversations";

            //Act
            HttpResponseMessage response = await _client.GetAsync(uri);

            //Assert
            ((int) response.StatusCode).Should().Be(405);
        }


    }
}
