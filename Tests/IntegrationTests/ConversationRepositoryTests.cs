using Application;
using Application.Features.Conversations;
using Application.Features.Conversations.Interfaces;
using Application.Features.Chatter;
using Dapper;
using FluentAssertions;
using System.Data;
using Tests.IntegrationTests.Fixtures;
using Xunit;

namespace Tests.IntegrationTests
{
    [Collection("DockerDatabaseCollection")]
    public class ConversationRepositoryTests
    {
        private readonly ConversationRepository _subject;

        private readonly IConnectionFactory _connectionFactory;

        private readonly ChatterId _currentChatterId;
        private readonly ChatterId _someOtherChatterId;
        private int _numberOfCreatedUsers;

        public ConversationRepositoryTests(
            IntegrationTestApplicationFactory factory)
        {
            _numberOfCreatedUsers = 0;
            _currentChatterId = new ChatterId();
            _someOtherChatterId = new ChatterId();

            ConversationRepository? subject = (ConversationRepository?) factory.Services.GetService(typeof(IConversationRepository));
            IConnectionFactory? connectionFactory = (IConnectionFactory?) factory.Services.GetService(typeof(IConnectionFactory));

            if (subject is null || connectionFactory is null)
            {
                throw new ApplicationException("Test dependencies not found in DI container");
            }

            _subject = subject;
            _connectionFactory = connectionFactory;

            ClearDatabaseData();
        }

        [Fact]
        public void GetByIdAsync_GivenNonExistantConversationId_Throws()
        {
            //Arrange
            ConversationId conversationId = new();

            //Act
            Func<Task> get = async () =>
            {
                await _subject.GetByIdAsync(_currentChatterId, conversationId);
            };

            //Assert
            get.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsEmpty()
        {
            //Arrange

            //Act
            IEnumerable<Conversation> conversations = await _subject.GetAllAsync(_currentChatterId);

            //Assert
            conversations.Should().BeEmpty();
        }

        [Fact]
        public async Task SaveAsync_GivenConversationCreation_CreatesConversation()
        {
            //Arrange
            await InsertFakeUserIntoDatabase(_currentChatterId);
            await InsertFakeUserIntoDatabase(_someOtherChatterId);
            List<ChatterId> conversationMemberIds = new() { _currentChatterId, _someOtherChatterId };

            //Act
            Conversation conversationToCreate = Conversation.Start(
                _currentChatterId,
                conversationMemberIds,
                "Title");
            await _subject.SaveAsync(conversationToCreate);

            //Assert
            Conversation conversationInDatabase = await _subject.GetByIdAsync(_currentChatterId, conversationToCreate.Id);

            conversationInDatabase.Should().NotBeNull();
            conversationInDatabase.Title.Should().Be("Title");
            conversationInDatabase.ConversationMemberIds.Should().HaveCount(2);
            conversationInDatabase.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public async Task SaveAsync_WhenLeavingConversation_LeavesConversation()
        {
            //Arrange
            await InsertFakeUserIntoDatabase(_currentChatterId);
            await InsertFakeUserIntoDatabase(_someOtherChatterId);
            List<ChatterId> conversationMemberIds = new() { _currentChatterId, _someOtherChatterId };

            Conversation conversationToLeave = Conversation.Start(
                _currentChatterId,
                conversationMemberIds,
                "Title");

            //Act
            conversationToLeave.Leave(_currentChatterId);
            await _subject.SaveAsync(conversationToLeave);

            //Assert
            IEnumerable<Conversation> conversations = await _subject.GetAllAsync(_currentChatterId);
            conversations.Should().BeEmpty();
        }

        [Fact]
        public async Task SaveAsync_WhenLeavingConversation_RemovesLeavingUserFromConversation()
        {
            //Arrange
            await InsertFakeUserIntoDatabase(_currentChatterId);
            await InsertFakeUserIntoDatabase(_someOtherChatterId);
            List<ChatterId> conversationMemberIds = new() { _currentChatterId, _someOtherChatterId };

            Conversation conversationToLeave = Conversation.Start(
                _currentChatterId,
                conversationMemberIds,
                "Title");

            //Act
            conversationToLeave.Leave(_currentChatterId);
            await _subject.SaveAsync(conversationToLeave);

            //Assert
            IEnumerable<Conversation> conversations = await _subject.GetAllAsync(_someOtherChatterId);
            conversations.Single().ConversationMemberIds.Should().HaveCount(1);
        }

        [Fact]
        public async Task SaveAsync_WhenLeavingConversationWithOneMember_DeletesEntireConversation()
        {
            //Arrange
            await InsertFakeUserIntoDatabase(_currentChatterId);
            await InsertFakeUserIntoDatabase(_someOtherChatterId);
            List<ChatterId> conversationMemberIds = new() { _currentChatterId, _someOtherChatterId };

            Conversation conversationToLeave = Conversation.Start(
                _currentChatterId,
                conversationMemberIds,
                "Title");

            //Act
            conversationToLeave.Leave(_currentChatterId);
            conversationToLeave.Leave(_someOtherChatterId);
            await _subject.SaveAsync(conversationToLeave);

            //Assert
            (await ConversationsExistInDatabase()).Should().BeFalse();

        }

        [Fact]
        public async Task SaveAsync_WhenPostingAMessage_PostsMessage()
        {
            //Arrange
            await InsertFakeUserIntoDatabase(_currentChatterId);
            await InsertFakeUserIntoDatabase(_someOtherChatterId);
            List<ChatterId> conversationMemberIds = new() { _currentChatterId, _someOtherChatterId };

            Conversation conversationToPostIn = Conversation.Start(
                _currentChatterId,
                conversationMemberIds,
                "Title");

            //Act
            conversationToPostIn.PostMessage(_currentChatterId, "Text");
            await _subject.SaveAsync(conversationToPostIn);

            //Assert
            Conversation conversationInDatabase = await _subject.GetByIdAsync(_currentChatterId, conversationToPostIn.Id);
            conversationInDatabase.LoadedMessages.Should().HaveCount(1);
        }

        [Fact]
        public async Task SaveAsync_WhenDeletingAMessage_DeletesMessage()
        {
            //Arrange
            await InsertFakeUserIntoDatabase(_currentChatterId);
            await InsertFakeUserIntoDatabase(_someOtherChatterId);
            List<ChatterId> conversationMemberIds = new() { _currentChatterId, _someOtherChatterId };

            Conversation conversation = Conversation.Start(
                _currentChatterId,
                conversationMemberIds,
                "Title");
            conversation.PostMessage(_currentChatterId, "Text");
            Message message = conversation.LoadedMessages.Single();

            //Act
            conversation.DeleteMessage(_currentChatterId, message.Id);
            await _subject.SaveAsync(conversation);

            //Assert
            Conversation conversationInDatabase = await _subject.GetByIdAsync(_currentChatterId, conversation.Id);
            conversationInDatabase.LoadedMessages.Should().BeEmpty();
        }

        [Fact]
        public async Task SaveAsync_WhenAddingAConversationMember_AddsMember()
        {
            //Arrange
            ChatterId yetAnotherChatter = new();
            await InsertFakeUserIntoDatabase(_currentChatterId);
            await InsertFakeUserIntoDatabase(_someOtherChatterId);
            await InsertFakeUserIntoDatabase(yetAnotherChatter);
            List<ChatterId> conversationMemberIds = new() { _currentChatterId, _someOtherChatterId };

            Conversation conversation = Conversation.Start(
                _currentChatterId,
                conversationMemberIds,
                "Title");

            //Act
            conversation.AddMember(_currentChatterId, yetAnotherChatter);
            await _subject.SaveAsync(conversation);

            //Assert
            Conversation conversationInDatabase = await _subject.GetByIdAsync(_currentChatterId, conversation.Id);
            conversationInDatabase.ConversationMemberIds.Should().HaveCount(3);
        }

        [Fact]
        public async Task SaveAsync_WhenKickingAConversationMember_RemovesMemberFromList()
        {
            //Arrange
            await InsertFakeUserIntoDatabase(_currentChatterId);
            await InsertFakeUserIntoDatabase(_someOtherChatterId);
            List<ChatterId> conversationMemberIds = new() { _currentChatterId, _someOtherChatterId };

            Conversation conversation = Conversation.Start(
                _currentChatterId,
                conversationMemberIds,
                "Title");

            //Act
            conversation.KickMember(_currentChatterId, _someOtherChatterId);
            await _subject.SaveAsync(conversation);

            //Assert
            Conversation conversationInDatabase = await _subject.GetByIdAsync(_currentChatterId, conversation.Id);
            conversationInDatabase.ConversationMemberIds.Should().HaveCount(1);
        }

        private async Task<bool> ConversationsExistInDatabase()
        {
            using IDbConnection connection = _connectionFactory.GetConnection(ConnectionType.SqlConnection);
            connection.Open();

            IEnumerable<Conversation> conversations = await connection.QueryAsync<Conversation>(
                """
                SELECT * FROM SocialMediaWebsite.dbo.Conversations;
                """);

            return conversations.Any();
        }

        private async Task InsertFakeUserIntoDatabase(ChatterId id)
        {
            using IDbConnection connection = _connectionFactory.GetConnection(ConnectionType.SqlConnection);
            connection.Open();

            await connection.ExecuteAsync(
                """
                INSERT INTO SocialMediaWebsite.dbo.Users
                (Id, UserName, Email, PasswordHash)
                VALUES
                (@Id, @UserName, @Email, @PasswordHash);
                """,
                new
                {
                    Id = id,
                    UserName = $"UserWithId{_numberOfCreatedUsers}",
                    Email = $"user{_numberOfCreatedUsers}@email.com",
                    PasswordHash = $"dummyHash{_numberOfCreatedUsers}"
                });
            _numberOfCreatedUsers++;
        }

        private void ClearDatabaseData()
        {
            using IDbConnection connection = _connectionFactory.GetConnection(ConnectionType.SqlConnection);
            connection.Open();

            connection.Execute(
                """
                EXEC sys.sp_msforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';
                EXEC sys.sp_msforeachtable 'DELETE FROM ?';
                EXEC sys.sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'
                """);
        }

    }
}
