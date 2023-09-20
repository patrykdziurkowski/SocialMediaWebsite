using Application.Features.Authentication;
using Application;
using Application.Features.Chat;
using Application.Features.Chat.Events;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Dapper;
using FluentResults;
using Application.Features.Chat.Interfaces;

namespace Tests.IntegrationTests
{
    public class ChatRepositoryTests : IClassFixture<IntegrationTestApplicationFactory>
    {
        private readonly ChatRepository _subject;

        private readonly IConnectionFactory _connectionFactory;

        private readonly ChatterId _currentChatterId;
        private readonly ChatterId _someOtherChatterId;
        private int _numberOfCreatedUsers;

        public ChatRepositoryTests(
            IntegrationTestApplicationFactory factory)
        {
            _numberOfCreatedUsers = 0;
            _currentChatterId = new ChatterId();
            _someOtherChatterId = new ChatterId();

            ChatRepository? subject = (ChatRepository?) factory.Services.GetService(typeof(IChatRepository));
            IConnectionFactory? connectionFactory = (IConnectionFactory?)factory.Services.GetService(typeof(IConnectionFactory));

            if (subject is null || connectionFactory is null)
            {
                throw new ApplicationException("Test dependencies not found in DI container");
            }  

            _subject = subject;
            _connectionFactory = connectionFactory;

            ClearDatabaseData();
        }

        [Fact]
        public async Task GetAsync_GivenUserId_ReturnsAValidChat()
        {
            //Arrange

            //Act
            Chat chat = await _subject.GetAsync(_currentChatterId);

            //Assert
            chat.Conversations.Should().BeEmpty(); 
            chat.DomainEvents.Should().BeEmpty();
            chat.CurrentChatterId.Should().Be(_currentChatterId);
        }

        [Fact]
        public async Task SaveAsync_GivenConversationCreation_CreatesConversation()
        {
            //Arrange
            await InsertFakeUserIntoDatabase(_currentChatterId);
            await InsertFakeUserIntoDatabase(_someOtherChatterId);
            Chat chat = await _subject.GetAsync(_currentChatterId);

            List<ChatterId> conversationMemberIds = new() { _currentChatterId, _someOtherChatterId };

            //Act
            chat.CreateConversation(conversationMemberIds, "Title");
            await _subject.SaveAsync(chat);

            //Assert
            Chat resultChat = await _subject.GetAsync(_currentChatterId);

            resultChat.Conversations.Should().HaveCount(1);
            resultChat.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public async Task SaveAsync_WhenLeavingConversation_LeavesConversation()
        {
            //Arrange
            Chat chat = await SetupConversationWithUsers();

            //Act
            Conversation conversationToLeave = chat.Conversations.Single();
            chat.LeaveConversation(conversationToLeave.Id);
            await _subject.SaveAsync(chat);

            //Assert
            Chat resultChat = await _subject.GetAsync(_currentChatterId);

            resultChat.Conversations.Should().BeEmpty();
            resultChat.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public async Task SaveAsync_WhenLeavingConversation_RemovesLeavingUserFromConversation()
        {
            //Arrange
            Chat chat = await SetupConversationWithUsers();

            //Act
            Conversation conversationToLeave = chat.Conversations.Single();
            chat.LeaveConversation(conversationToLeave.Id);
            await _subject.SaveAsync(chat);

            //Assert
            Chat resultChat = await _subject.GetAsync(_someOtherChatterId);

            resultChat.Conversations.Single().ConversationMemberIds.Should().HaveCount(1);
            resultChat.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public async Task SaveAsync_WhenLeavingConversationWithOneMember_DeletesEntireConversation()
        {
            //Arrange
            Chat chat = await SetupConversationWithOneUser();

            //Act
            Conversation conversationToLeave = chat.Conversations.Single();
            chat.LeaveConversation(conversationToLeave.Id);
            await _subject.SaveAsync(chat);

            //Assert
            Chat resultChat = await _subject.GetAsync(_currentChatterId);

            resultChat.Conversations.Should().BeEmpty();
            resultChat.DomainEvents.Should().BeEmpty();
            (await ConversationsExistInDatabase()).Should().BeFalse();
        }

        [Fact]
        public async Task SaveAsync_WhenPostingAMessage_PostsMessage()
        {
            //Arrange
            Chat chat = await SetupConversationWithUsers();

            //Act
            Conversation conversationToPostIn = chat.Conversations.Single();
            chat.PostMessage(conversationToPostIn.Id, "Message text");
            await _subject.SaveAsync(chat);

            //Assert
            Chat resultChat = await _subject.GetAsync(_currentChatterId);
            resultChat.Conversations.Single().LoadedMessages.Should().HaveCount(1);
            resultChat.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public async Task SaveAsync_WhenDeletingAMessage_DeletesMessage()
        {
            //Arrange
            Chat chat = await SetupConversationWithUsers();
            Chat chatWithAMessage = await PostAMessageInConversation(chat);

            //Act
            chatWithAMessage.DeleteMessage(
                chatWithAMessage.Conversations.Single().Id,
                chatWithAMessage.Conversations.Single().LoadedMessages.Single().Id);
            await _subject.SaveAsync(chatWithAMessage);

            //Assert
            Chat resultChat = await _subject.GetAsync(_currentChatterId);
            resultChat.Conversations.Single().LoadedMessages.Should().BeEmpty();
            resultChat.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public async Task SaveAsync_WhenAddingAConversationMember_AddsMember()
        {
            //Arrange
            ChatterId chatterToAddId = new();
            
            await InsertFakeUserIntoDatabase(chatterToAddId);
            Chat chat = await SetupConversationWithUsers();
            ConversationId conversationToAddToId = chat.Conversations.Single().Id;

            //Act
            chat.AddMemberToConversation(
                conversationToAddToId,
                chatterToAddId);
            await _subject.SaveAsync(chat);

            //Assert
            chat = await _subject.GetAsync(_currentChatterId);
            chat.Conversations.Single().ConversationMemberIds.Should().Contain(chatterToAddId);
        }

        [Fact]
        public async Task SaveAsync_WhenKickingAConversationMember_RemovesMemberFromList()
        {
            //Arrange
            Chat chat = await SetupConversationWithUsers();
            ConversationId conversationToKickFrom = chat.Conversations.Single().Id;

            //Act
            chat.KickMemberFromConversation(
                conversationToKickFrom,
                _someOtherChatterId);
            await _subject.SaveAsync(chat);

            //Assert
            chat = await _subject.GetAsync(_currentChatterId);
            chat.Conversations.Single().ConversationMemberIds.Should().NotContain(_someOtherChatterId);
        }




        private async Task<Chat> PostAMessageInConversation(Chat chat)
        {
            Conversation conversationToPostIn = chat.Conversations.Single();
            chat.PostMessage(conversationToPostIn.Id, "Message text");
            await _subject.SaveAsync(chat);
            return await _subject.GetAsync(_currentChatterId);
        }

        private async Task<Chat> SetupConversationWithOneUser()
        {
            await InsertFakeUserIntoDatabase(_currentChatterId);
            Chat chat = await _subject.GetAsync(_currentChatterId);

            List<ChatterId> conversationMemberIds = new() { _currentChatterId };
            chat.CreateConversation(conversationMemberIds, "Title");
            await _subject.SaveAsync(chat);
            chat = await _subject.GetAsync(_currentChatterId);
            return chat;
        }

        private async Task<Chat> SetupConversationWithUsers()
        {
            await InsertFakeUserIntoDatabase(_currentChatterId);
            await InsertFakeUserIntoDatabase(_someOtherChatterId);
            Chat chat = await _subject.GetAsync(_currentChatterId);

            List<ChatterId> conversationMemberIds = new() { _currentChatterId, _someOtherChatterId };
            chat.CreateConversation(conversationMemberIds, "Title");
            await _subject.SaveAsync(chat);
            chat = await _subject.GetAsync(_currentChatterId);
            return chat;
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
