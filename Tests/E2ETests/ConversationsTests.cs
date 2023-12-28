using Application.Features.Authentication;
using FluentAssertions;
using Newtonsoft.Json;
using Tests.E2ETests.Fixtures;
using Xunit;
using Xunit.Priority;

namespace Tests.E2ETests
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    [Collection("WebServerTests")]
    public class ConversationsTests : IClassFixture<WebServerHostService>, IClassFixture<ConversationTestsFixture>
    {
        private static readonly HttpClient _client = new(new HttpClientHandler() { CookieContainer = new System.Net.CookieContainer() });

        private readonly WebServerHostService _hostService;
        private readonly ConversationTestsFixture _testContext;

        public ConversationsTests(
            WebServerHostService hostService,
            ConversationTestsFixture testContext)
        {
            _hostService = hostService;
            _testContext = testContext;
        }


        [Fact, Priority(0)]
        public async Task ConversationsEndpoint_GivenUnauthorizedClient_Returns405()
        {
            //Arrange   
            string uri = "http://localhost:8080/Conversations";

            //Act
            HttpResponseMessage response = await _client.GetAsync(uri);

            //Assert
            ((int) response.StatusCode).Should().Be(405);
        }

        [Fact, Priority(5)]
        public async Task GetConversations_GivenNewlyCreatedUser_Returns200()
        {
            //Arrange   
            _testContext.User1Id = await RegisterNewUser();
            await Login();

            string uri = "http://localhost:8080/Conversations";

            //Act
            HttpResponseMessage response = await _client.GetAsync(uri);

            //Assert
            ((int) response.StatusCode).Should().Be(200);
        }

        [Fact, Priority(10)]
        public async Task StartConversation_GivenValidConversation_Returns201()
        {
            //Arrange
            _testContext.User2Id = await RegisterSecondNewUser();

            FormUrlEncodedContent form = new(new[]
            {
                new KeyValuePair<string, string>("ConversationMemberIds[0]", _testContext.User1Id!.ToString()),
                new KeyValuePair<string, string>("ConversationMemberIds[1]", _testContext.User2Id!.ToString()),
                new KeyValuePair<string, string>("Title", "ConversationTitle"),
                new KeyValuePair<string, string>("Description", "My conversation!")
            });
            string uri = "http://localhost:8080/Conversations";

            //Act
            HttpResponseMessage response = await _client.PostAsync(uri, form);

            //Assert
            ((int) response.StatusCode).Should().Be(201);
        }



        private async Task<UserId> RegisterSecondNewUser()
        {
            FormUrlEncodedContent form = new(new[]
            {
                new KeyValuePair<string, string>("UserName", "SomeOtherGuy123"),
                new KeyValuePair<string, string>("Email", "some@otherguy.com"),
                new KeyValuePair<string, string>("Password", "S3curePwd!"),
            });
            string uri = "http://localhost:8080/Authentication/Register";

            HttpResponseMessage response = await _client.PostAsync(uri, form);

            string responseContent = await response.Content.ReadAsStringAsync();
            UserDto? createdUser = JsonConvert.DeserializeObject<UserDto>(responseContent)
                ?? throw new ArgumentNullException();

            return new UserId(createdUser.Id);
        }

        private async Task<UserId> RegisterNewUser()
        {
            FormUrlEncodedContent form = new(new[]
            {
                new KeyValuePair<string, string>("UserName", "JohnSmith123"),
                new KeyValuePair<string, string>("Email", "john@smith.com"),
                new KeyValuePair<string, string>("Password", "P@ssword1!"),
            });
            string uri = "http://localhost:8080/Authentication/Register";

            HttpResponseMessage response = await _client.PostAsync(uri, form);

            string responseContent = await response.Content.ReadAsStringAsync();
            UserDto? createdUser = JsonConvert.DeserializeObject<UserDto>(responseContent)
                ?? throw new ArgumentNullException();

            return new UserId(createdUser.Id);
        }

        private async Task Login()
        {
            FormUrlEncodedContent form = new(new[]
            {
                new KeyValuePair<string, string>("UserName", "JohnSmith123"),
                new KeyValuePair<string, string>("Password", "P@ssword1!"),
            });
            string uri = "http://localhost:8080/Authentication/Login";

            await _client.PostAsync(uri, form);
        }

    }

}
