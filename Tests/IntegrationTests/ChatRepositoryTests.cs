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

namespace Tests.IntegrationTests
{
    public class ChatRepositoryTests : IClassFixture<IntegrationTestApplicationFactory>
    {
        private readonly ChatRepository _subject;

        private readonly IConnectionFactory _connectionFactory;

        public ChatRepositoryTests(
            IntegrationTestApplicationFactory factory)
        {
            ChatRepository? subject = (ChatRepository?) factory.Services.GetService(typeof(ChatRepository));
            IConnectionFactory? connectionFactory = (IConnectionFactory?)factory.Services.GetService(typeof(IConnectionFactory));

            if (subject is null || connectionFactory is null)
            {
                throw new ArgumentNullException("Test dependencies not found in DI container");
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
            Chat chat = await _subject.GetAsync(1);

            //Assert
            chat.Conversations.Should().BeEmpty(); 
            chat.DomainEvents.Should().BeEmpty();
            chat.ChatterId.Should().Be(1);
        }

        [Fact]
        public async Task SaveAsync_GivenConversationCreation_CreatesConversation()
        {
            //Arrange
            await InsertFakeUserIntoDatabase(1);
            await InsertFakeUserIntoDatabase(2);
            Chat chat = await _subject.GetAsync(1);

            List<Chatter> conversationMembers = new()
            {
                    new Chatter(1, "UserWithId1", DateTimeOffset.MinValue),
                    new Chatter(2, "UserWithId2", DateTimeOffset.MinValue)
            };

            //Act
            chat.CreateConversation(conversationMembers, "Title");
            await _subject.SaveAsync(chat);

            //Assert
            Chat resultChat = await _subject.GetAsync(1);

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
            chat.LeaveConversation((int)conversationToLeave.Id!);
            await _subject.SaveAsync(chat);

            //Assert
            Chat resultChat = await _subject.GetAsync(1);

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
            chat.LeaveConversation((int) conversationToLeave.Id!);
            await _subject.SaveAsync(chat);

            //Assert
            Chat resultChat = await _subject.GetAsync(2);

            resultChat.Conversations.Single().ConversationMembers.Should().HaveCount(1);
            resultChat.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public async Task SaveAsync_WhenPostingAMessage_PostsMessage()
        {
            //Arrange
            Chat chat = await SetupConversationWithUsers();

            //Act
            Conversation conversationToPostIn = chat.Conversations.Single();
            chat.PostMessage((int) conversationToPostIn.Id!, "Message text");
            await _subject.SaveAsync(chat);

            //Assert
            Chat resultChat = await _subject.GetAsync(1);
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
                (int) chatWithAMessage.Conversations.Single().Id!,
                chatWithAMessage.Conversations.Single().LoadedMessages.Single().Id);
            await _subject.SaveAsync(chatWithAMessage);

            //Assert
            Chat resultChat = await _subject.GetAsync(1);
            resultChat.Conversations.Single().LoadedMessages.Should().BeEmpty();
            resultChat.DomainEvents.Should().BeEmpty();
        }

        private async Task<Chat> PostAMessageInConversation(Chat chat)
        {
            Conversation conversationToPostIn = chat.Conversations.Single();
            chat.PostMessage((int) conversationToPostIn.Id!, "Message text");
            await _subject.SaveAsync(chat);
            return await _subject.GetAsync(1);
        }

        private async Task<Chat> SetupConversationWithUsers()
        {
            await InsertFakeUserIntoDatabase(1);
            await InsertFakeUserIntoDatabase(2);
            Chat chat = await _subject.GetAsync(1);

            List<Chatter> conversationMembers = new()
            {
                    new Chatter(1, "UserWithId1", DateTimeOffset.MinValue),
                    new Chatter(2, "UserWithId2", DateTimeOffset.MinValue)
            };
            chat.CreateConversation(conversationMembers, "Title");
            await _subject.SaveAsync(chat);
            chat = await _subject.GetAsync(1);
            return chat;
        }

        private async Task InsertFakeUserIntoDatabase(int id)
        {
            using IDbConnection connection = _connectionFactory.GetConnection(ConnectionType.SqlConnection);
            connection.Open();

            await connection.ExecuteAsync(
                """
                SET IDENTITY_INSERT SocialMediaWebsite.dbo.Users ON;
                INSERT INTO SocialMediaWebsite.dbo.Users
                (Id, UserName, Email, PasswordHash)
                VALUES
                (@Id, @UserName, @Email, @PasswordHash);
                SET IDENTITY_INSERT SocialMediaWebsite.dbo.Users OFF
                """,
                new
                {
                    Id = id,
                    UserName = $"UserWithId{id}",
                    Email = $"user{id}@email.com",
                    PasswordHash = $"dummyHash{id}"
                });
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
