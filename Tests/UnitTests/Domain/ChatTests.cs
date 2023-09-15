using Application.Features.Chat;
using Application.Features.Chat.Events;
using Application.Features.Shared;
using FluentAssertions;
using FluentResults;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Domain
{
    public class ChatTests
    {
        private Chat? _subject;

        [Fact]
        public void CreateConversation_AddsNewConversationToList()
        {
            //Arrange
            _subject = new(1, new List<Conversation>());

            //Act
            _subject.CreateConversation(new List<int>(), "Title", "Description");

            //Assert
            _subject.Conversations.Should().HaveCount(1);
            _subject.Conversations.Single().Title.Should().Be("Title");
            _subject.Conversations.Single().Description.Should().Be("Description");
        }

        [Fact]
        public void CreateConversation_AddsNewConversationWithConversationMembers()
        {
            //Arrange
            _subject = new(1, new List<Conversation>());
            List<int> conversationMemberIds = new() { 1, 2, 3 };

            //Act
            _subject.CreateConversation(conversationMemberIds, "Title", "Description");

            //Assert
            _subject.Conversations.Single().ConversationMemberIds.Should().HaveCount(conversationMemberIds.Count);

        }

        [Fact]
        public void LeaveConversation_RemovesChatterFromConversation_WhenValidId()
        {
            //Arrange
            const int ConversationId = 50;
            const int LeavingChatterId = 1;

            List<Conversation> conversations = new()
            {
                CreateSampleConversationWithChattersAndId(ConversationId)
            }; 

            _subject = new(LeavingChatterId, conversations);
            IEnumerable<int> conversationMemberIds = _subject.Conversations.Single().ConversationMemberIds;

            //Act
            _subject.LeaveConversation(ConversationId);

            //Assert
            conversationMemberIds.Should().NotContain(LeavingChatterId);
            conversationMemberIds.Should().HaveCount(2);
            conversations.Should().BeEmpty();
        }

        [Fact]
        public void LeaveConversation_ThrowsException_WhenInvalidId()
        {
            //Arrange
            const int LeavingChatterId = 1;
            List<Conversation> conversations = new()
            {
                CreateSampleConversationWithChattersAndId(50)
            };

            _subject = new(LeavingChatterId, conversations);
            IEnumerable<int> conversationMemberIds = _subject.Conversations.Single().ConversationMemberIds;

            //Act
            _subject
                .Invoking(m => m.LeaveConversation(999))
                .Should().Throw<InvalidOperationException>();

            //Assert
            conversationMemberIds.Should().Contain(LeavingChatterId);
            conversationMemberIds.Should().HaveCount(3);
            conversations.Should().NotBeEmpty();
        }


        [Fact]
        public void PostMessage_GivenExistingConversationId_AddsAMessageToConversation()
        {
            //Arrange
            const int CurrentChatterId = 1;
            List<Conversation> conversations = new()
            {
                CreateSampleConversationWithChattersAndId(50)
            };

            _subject = new(CurrentChatterId, conversations);
            Conversation conversation = _subject.Conversations.Single(c => c.Id == 50);

            //Act
            _subject.PostMessage(50, "Text");

            //Assert
            conversation.TotalMessageCount.Should().Be(1);
            conversation.LoadedMessages.Should().HaveCount(1);
            conversation.LoadedMessages.Single().Text.Should().Be("Text");
        }

        [Fact]
        public void PostMessage_GivenNonExistantConversationId_ThrowsException()
        {
            //Arrange
            const int CurrentChatterId = 1;
            List<Conversation> conversations = new()
            {
                CreateSampleConversationWithChattersAndId(50)
            };

            _subject = new(CurrentChatterId, conversations);
            Conversation conversation = _subject.Conversations.Single(c => c.Id == 50);

            //Act
            _subject
                .Invoking(m => m.PostMessage(999, "Text"))
                .Should().Throw<InvalidOperationException>();

            //Assert
            conversation.TotalMessageCount.Should().Be(0);
            conversation.LoadedMessages.Should().BeEmpty();
        }

        [Fact]
        public void DeleteMessage_GivenExistingConversationAndMessage_RemovesMessage()
        {
            //Arrange
            const int CurrentChatterId = 1;
            List<Conversation> conversations = new()
            {
                CreateSampleConversationWithChattersAndId(50)
            };
            conversations.Single().LoadedMessages.Add(
                new Message(
                    600,
                    CurrentChatterId,
                    "Text",
                    new DateTimeOffset()));

            _subject = new(CurrentChatterId, conversations);
            Conversation conversation = _subject.Conversations.Single(c => c.Id == 50);

            //Act
            _subject.DeleteMessage(50, 600);

            //Assert
            conversation.LoadedMessages.Should().BeEmpty();
        }

        [Fact]
        public void DeleteMessage_GivenNonExistantConversation_ThrowsException()
        {
            //Arrange
            const int CurrentChatterId = 1;
            List<Conversation> conversations = new()
            {
                CreateSampleConversationWithChattersAndId(50)
            };
            conversations.Single().LoadedMessages.Add(
                new Message(
                    600,
                    CurrentChatterId,
                    "Text",
                    new DateTimeOffset()));

            _subject = new(CurrentChatterId, conversations);
            Conversation conversation = _subject.Conversations.Single(c => c.Id == 50);

            //Act
            _subject
                .Invoking(m => m.DeleteMessage(70, 600))
                .Should().Throw<InvalidOperationException>();

            //Assert
            conversation.LoadedMessages.Should().HaveCount(1);
        }

        [Fact]
        public void DeleteMessage_GivenNonExistantMessage_ThrowsException()
        {
            //Arrange
            const int CurrentChatterId = 1;
            List<Conversation> conversations = new()
            {
                CreateSampleConversationWithChattersAndId(50)
            };
            conversations.Single().LoadedMessages.Add(
                new Message(
                    600,
                    CurrentChatterId,
                    "Text",
                    new DateTimeOffset()));

            _subject = new(CurrentChatterId, conversations);
            Conversation conversation = _subject.Conversations.Single(c => c.Id == 50);

            //Act
            _subject
                .Invoking(m => m.DeleteMessage(50, 700))
                .Should().Throw<InvalidOperationException>();

            //Assert
            conversation.LoadedMessages.Should().HaveCount(1);
        }

        [Fact]
        public void DeleteMessage_WhenMessageDoesntBelongToCurrentUser_ThrowsException()
        {
            //Arrange
            const int CurrentChatterId = 1;
            const int SomeOtherChatterId = 55;
            List<Conversation> conversations = new()
            {
                CreateSampleConversationWithChattersAndId(50)
            };
            conversations.Single().LoadedMessages.Add(
                new Message(
                    600,
                    SomeOtherChatterId,
                    "Text",
                    new DateTimeOffset()));

            _subject = new(CurrentChatterId, conversations);
            Conversation conversation = _subject.Conversations.Single(c => c.Id == 50);

            //Act & Assert
            _subject
                .Invoking(m => m.DeleteMessage(50, 600))
                .Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void RaiseDomainEvent_AddsEventToList()
        {
            //Arrange
            _subject = new(1, new List<Conversation>());

            //Act
            _subject.RaiseDomainEvent(new ConversationLeftEvent(1, 1));

            //Assert
            _subject.DomainEvents.Should().HaveCount(1);

        }

        [Fact]
        public void ClearDomainEvents_RemovesAllEventsFromList()
        {
            //Arrange
            _subject = new(1, new List<Conversation>());
            _subject.RaiseDomainEvent(new ConversationLeftEvent(1, 1));
            _subject.RaiseDomainEvent(new ConversationLeftEvent(2, 1));

            int numberOfEventsBeforeClearing = _subject.DomainEvents.Count();

            //Act
            _subject.ClearDomainEvents();

            //Assert
            numberOfEventsBeforeClearing.Should().Be(2);
            _subject.DomainEvents.Should().BeEmpty();
        }


        [Fact]
        public void AddMemberToConversation_AddsChatterId_ToConversationMemberIds()
        {
            //Arrange
            Conversation conversation = CreateSampleConversationWithChattersAndId(50);
            _subject = new(1, new List<Conversation>() { conversation });

            //Act
            Result result = _subject.AddMemberToConversation((int)conversation.Id!, 4);

            //Assert
            conversation.ConversationMemberIds.Should().Contain(4);
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void AddMemberToConversation_ReturnsFail_IfMemberAlreadyInConversation()
        {
            //Arrange
            Conversation conversation = CreateSampleConversationWithChattersAndId(50);
            _subject = new(1, new List<Conversation>() { conversation });

            //Act
            Result result = _subject.AddMemberToConversation((int) conversation.Id!, 2);

            //Assert
            conversation.ConversationMemberIds.Should().HaveCount(3);
            result.IsFailed.Should().BeTrue();
        }

        [Fact]
        public void AddMemberToConversation_ReturnsFail_IfCurrentChatterIsNotConversationOwner()
        {
            //Arrange
            const int CurrentChatterId = 2;

            Conversation conversation = CreateSampleConversationWithChattersAndId(50);
            _subject = new(CurrentChatterId, new List<Conversation>() { conversation });

            int ChatOwnerId = conversation.OwnerChatterId;

            //Act
            Result result = _subject.AddMemberToConversation((int)conversation.Id!, 4);

            //Assert
            CurrentChatterId.Should().NotBe(ChatOwnerId);
            conversation.ConversationMemberIds.Should().HaveCount(3);
            result.IsFailed.Should().BeTrue();
        }

        [Fact]
        public void KickMemberFromConversation_ReturnsFail_WhenCurrentChatterIsNotConversationOwner()
        {
            //Arrange
            const int CurrentChatterId = 2;

            Conversation conversation = CreateSampleConversationWithChattersAndId(50);
            _subject = new(CurrentChatterId, new List<Conversation>() { conversation });

            int ChatOwnerId = conversation.OwnerChatterId;

            //Act
            Result result = _subject.KickMemberFromConversation((int) conversation.Id!, 1);

            //Assert
            CurrentChatterId.Should().NotBe(ChatOwnerId);
            result.IsFailed.Should().BeTrue();
            conversation.ConversationMemberIds.Should().Contain(1);
        }

        [Fact]
        public void KickMemberFromConversation_Throws_WhenMemberNotInConversation()
        {
            //Arrange
            int idOfChatterNotInConversation = 22;
            Conversation conversation = CreateSampleConversationWithChattersAndId(50);
            _subject = new(1, new List<Conversation>() { conversation });

            //Act & Assert
            _subject
                .Invoking(m => m.KickMemberFromConversation(
                    (int) conversation.Id!,
                    idOfChatterNotInConversation))
                .Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void KickMemberFromConversation_GivenConversationMember_RemovesThemFromList()
        {
            //Arrange
            Conversation conversation = CreateSampleConversationWithChattersAndId(50);
            _subject = new(1, new List<Conversation>() { conversation });

            //Act
            _subject.KickMemberFromConversation((int) conversation.Id!, 2);

            //Assert
            conversation.ConversationMemberIds.Should().NotContain(2);
        }


        private static Conversation CreateSampleConversationWithChattersAndId(int id)
        {
            return new(
                id,
                new DateTimeOffset(),
                0,
                1,
                new List<Message>(),
                new List<int>() { 1, 2, 3 },
                "Title");
        }

    }
}
