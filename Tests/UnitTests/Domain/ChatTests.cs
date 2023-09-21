using Application.Features.Chat;
using Application.Features.Chat.Events;
using Application.Features.Chatter;
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
        private readonly ChatterId _currentChatterId;
        private readonly ChatterId _chatterInConversationId;
        private readonly ChatterId _chatterNotInConversationId;

        public ChatTests()
        {
            _currentChatterId = new ChatterId();
            _chatterInConversationId = new ChatterId();
            _chatterNotInConversationId = new ChatterId();
        }

        [Fact]
        public void CreateConversation_AddsNewConversationToList()
        {
            //Arrange
            _subject = new(_currentChatterId, new List<Conversation>());

            //Act
            _subject.CreateConversation(new List<ChatterId>(), "Title", "Description");

            //Assert
            _subject.Conversations.Should().HaveCount(1);
            _subject.Conversations.Single().Title.Should().Be("Title");
            _subject.Conversations.Single().Description.Should().Be("Description");
        }

        [Fact]
        public void CreateConversation_AddsNewConversationWithConversationMembers()
        {
            //Arrange
            _subject = new(_currentChatterId, new List<Conversation>());
            List<ChatterId> conversationMemberIds = new()
            { 
                _currentChatterId,
                new ChatterId(),
                new ChatterId()
            };

            //Act
            _subject.CreateConversation(conversationMemberIds, "Title", "Description");

            //Assert
            _subject.Conversations.Single().ConversationMemberIds.Should().HaveCount(conversationMemberIds.Count);

        }

        [Fact]
        public void LeaveConversation_RemovesChatterFromConversation_WhenValidId()
        {
            //Arrange
            List<Conversation> conversations = new()
            {
                CreateSampleConversationWithChatters(_currentChatterId)
            }; 

            _subject = new(_currentChatterId, conversations);
            IEnumerable<ChatterId> conversationMemberIds = _subject.Conversations.Single().ConversationMemberIds;

            //Act
            _subject.LeaveConversation(conversations.Single().Id);

            //Assert
            conversationMemberIds.Should().NotContain(_currentChatterId);
            conversationMemberIds.Should().HaveCount(1);
            conversations.Should().BeEmpty();
        }

        [Fact]
        public void LeaveConversation_ThrowsException_WhenInvalidId()
        {
            //Arrange
            List<Conversation> conversations = new()
            {
                CreateSampleConversationWithChatters(new ChatterId())
            };

            _subject = new(_currentChatterId, conversations);
            IEnumerable<ChatterId> conversationMemberIds = _subject.Conversations.Single().ConversationMemberIds;

            //Act
            _subject
                .Invoking(m => m.LeaveConversation(new ConversationId(Guid.Empty)))
                .Should().Throw<InvalidOperationException>();

            //Assert
            conversationMemberIds.Should().Contain(_currentChatterId);
            conversationMemberIds.Should().HaveCount(2);
            conversations.Should().NotBeEmpty();
        }


        [Fact]
        public void PostMessage_GivenExistingConversationId_AddsAMessageToConversation()
        {
            //Arrange
            List<Conversation> conversations = new()
            {
                CreateSampleConversationWithChatters(_currentChatterId)
            };

            _subject = new(_currentChatterId, conversations);
            Conversation conversation = _subject.Conversations.Single();

            //Act
            _subject.PostMessage(conversation.Id, "Text");

            //Assert
            conversation.TotalMessageCount.Should().Be(1);
            conversation.LoadedMessages.Should().HaveCount(1);
            conversation.LoadedMessages.Single().Text.Should().Be("Text");
        }

        [Fact]
        public void PostMessage_GivenNonExistantConversationId_ThrowsException()
        {
            //Arrange
            List<Conversation> conversations = new()
            {
                CreateSampleConversationWithChatters(_currentChatterId)
            };

            _subject = new(_currentChatterId, conversations);
            Conversation conversation = _subject.Conversations.Single();

            //Act
            _subject
                .Invoking(m => m.PostMessage(new ConversationId(Guid.Empty), "Text"))
                .Should().Throw<InvalidOperationException>();

            //Assert
            conversation.TotalMessageCount.Should().Be(0);
            conversation.LoadedMessages.Should().BeEmpty();
        }

        [Fact]
        public void DeleteMessage_GivenExistingConversationAndMessage_RemovesMessage()
        {
            //Arrange
            List<Conversation> conversations = new()
            {
                CreateSampleConversationWithChatters(_currentChatterId)
            };

            Message messageToDelete = new(
                    _currentChatterId,
                    "Text",
                    DateTimeOffset.MinValue);
            conversations.Single().LoadedMessages.Add(messageToDelete);

            _subject = new(_currentChatterId, conversations);
            Conversation conversation = _subject.Conversations.Single();

            //Act
            _subject.DeleteMessage(conversation.Id, messageToDelete.Id);

            //Assert
            conversation.LoadedMessages.Should().BeEmpty();
        }

        [Fact]
        public void DeleteMessage_GivenNonExistantConversation_ThrowsException()
        {
            //Arrange
            List<Conversation> conversations = new()
            {
                CreateSampleConversationWithChatters(_currentChatterId)
            };

            Message messageToDelete = new(
                _currentChatterId,
                "Text",
                DateTimeOffset.MinValue);
            conversations.Single().LoadedMessages.Add(messageToDelete);

            _subject = new(_currentChatterId, conversations);
            Conversation conversation = _subject.Conversations.Single();

            //Act
            _subject
                .Invoking(m => m.DeleteMessage(new ConversationId(Guid.Empty), messageToDelete.Id))
                .Should().Throw<InvalidOperationException>();

            //Assert
            conversation.LoadedMessages.Should().HaveCount(1);
        }

        [Fact]
        public void DeleteMessage_GivenNonExistantMessage_ThrowsException()
        {
            //Arrange
            List<Conversation> conversations = new()
            {
                CreateSampleConversationWithChatters(_currentChatterId)
            };

            Message messageToDelete = new(
                _currentChatterId,
                "Text",
                DateTimeOffset.MinValue);
            conversations.Single().LoadedMessages.Add(messageToDelete);

            _subject = new(_currentChatterId, conversations);
            Conversation conversation = _subject.Conversations.Single();

            //Act
            _subject
                .Invoking(m => m.DeleteMessage(conversation.Id, new MessageId(Guid.Empty)))
                .Should().Throw<InvalidOperationException>();

            //Assert
            conversation.LoadedMessages.Should().HaveCount(1);
        }

        [Fact]
        public void DeleteMessage_WhenMessageDoesntBelongToCurrentUser_ThrowsException()
        {
            //Arrange
            List<Conversation> conversations = new()
            {
                CreateSampleConversationWithChatters(_currentChatterId)
            };

            Message messageToDelete = new(
                _chatterInConversationId,
                "Text",
                DateTimeOffset.MinValue);
            conversations.Single().LoadedMessages.Add(messageToDelete);

            _subject = new(_currentChatterId, conversations);
            Conversation conversation = _subject.Conversations.Single();

            //Act & Assert
            _subject
                .Invoking(m => m.DeleteMessage(conversation.Id, messageToDelete.Id))
                .Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void AddMemberToConversation_AddsChatterId_ToConversationMemberIds()
        {
            //Arrange
            Conversation conversation = CreateSampleConversationWithChatters(_currentChatterId);
            _subject = new(_currentChatterId, new List<Conversation>() { conversation });

            //Act
            Result result = _subject.AddMemberToConversation(conversation.Id, _chatterNotInConversationId);

            //Assert
            conversation.ConversationMemberIds.Should().Contain(_chatterNotInConversationId);
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void AddMemberToConversation_ReturnsFail_IfMemberAlreadyInConversation()
        {
            //Arrange
            Conversation conversation = CreateSampleConversationWithChatters(_currentChatterId);
            _subject = new(_currentChatterId, new List<Conversation>() { conversation });

            //Act
            Result result = _subject.AddMemberToConversation(conversation.Id, _chatterInConversationId);

            //Assert
            conversation.ConversationMemberIds.Should().HaveCount(2);
            result.IsFailed.Should().BeTrue();
        }

        [Fact]
        public void AddMemberToConversation_ReturnsFail_IfCurrentChatterIsNotConversationOwner()
        {
            //Arrange
            Conversation conversation = CreateSampleConversationWithChatters(_chatterInConversationId);
            _subject = new(_currentChatterId, new List<Conversation>() { conversation });

            ChatterId chatOwnerId = conversation.OwnerChatterId;

            //Act
            Result result = _subject.AddMemberToConversation(conversation.Id, _chatterNotInConversationId);

            //Assert
            _currentChatterId.Should().NotBe(chatOwnerId);
            conversation.ConversationMemberIds.Should().HaveCount(2);
            result.IsFailed.Should().BeTrue();
        }

        [Fact]
        public void KickMemberFromConversation_ReturnsFail_WhenCurrentChatterIsNotConversationOwner()
        {
            //Arrange
            Conversation conversation = CreateSampleConversationWithChatters(_chatterInConversationId);
            _subject = new(_currentChatterId, new List<Conversation>() { conversation });

            ChatterId chatOwnerId = conversation.OwnerChatterId;

            //Act
            Result result = _subject.KickMemberFromConversation(conversation.Id, _chatterInConversationId);

            //Assert
            _currentChatterId.Should().NotBe(chatOwnerId);
            result.IsFailed.Should().BeTrue();
            conversation.ConversationMemberIds.Should().Contain(_chatterInConversationId);
        }

        [Fact]
        public void KickMemberFromConversation_Throws_WhenMemberNotInConversation()
        {
            //Arrange
            ConversationId conversationId = new();
            Conversation conversation = CreateSampleConversationWithChatters(_currentChatterId);
            _subject = new(_currentChatterId, new List<Conversation>() { conversation });

            //Act & Assert
            _subject
                .Invoking(m => m.KickMemberFromConversation(
                    conversationId,
                    _chatterNotInConversationId))
                .Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void KickMemberFromConversation_GivenConversationMember_RemovesThemFromList()
        {
            //Arrange
            Conversation conversation = CreateSampleConversationWithChatters(_currentChatterId);
            _subject = new(_currentChatterId, new List<Conversation>() { conversation });

            //Act
            _subject.KickMemberFromConversation(conversation.Id, _chatterInConversationId);

            //Assert
            conversation.ConversationMemberIds.Should().NotContain(_chatterInConversationId);
        }





        private Conversation CreateSampleConversationWithChatters(ChatterId conversationOwnerId)
        {
            return new Conversation(
                DateTimeOffset.MinValue,
                conversationOwnerId,
                new List<ChatterId>() { _currentChatterId, _chatterInConversationId },
                "Title");
        }

    }
}
