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
    public class ConversationTests
    {
        private Conversation? _subject;
        private readonly ChatterId _currentChatterId;
        private readonly ChatterId _chatterInConversationId;
        private readonly ChatterId _chatterNotInConversationId;

        public ConversationTests()
        {
            _currentChatterId = new ChatterId();
            _chatterInConversationId = new ChatterId();
            _chatterNotInConversationId = new ChatterId();
        }

        [Fact]
        public void CreateConversation_CreatesAConversation_WithDomainEvent()
        {
            //Arrange

            //Act
            _subject = Conversation.Create(
                _currentChatterId,
                new List<ChatterId>(),
                "Title");

            //Assert
            _subject.Title.Should().Be("Title");
            _subject.DomainEvents.Should().HaveCount(1);
            _subject.DomainEvents.Should().Contain(de => de is ConversationCreatedEvent);
        }

        [Fact]
        public void CreateConversation_AddsNewConversationWithConversationMembers()
        {
            //Arrange
            List<ChatterId> conversationMemberIds = new()
            { 
                _currentChatterId,
                new ChatterId(),
                new ChatterId()
            };

            //Act
            _subject = Conversation.Create(
                _currentChatterId,
                conversationMemberIds,
                "Title");

            //Assert
            _subject.ConversationMemberIds.Should().HaveCount(3);
        }

        [Fact]
        public void LeaveConversation_RemovesChatterFromConversation()
        {
            //Arrange
            _subject = CreateSampleConversationWithChatters(_currentChatterId);

            //Act
            _subject.Leave(_currentChatterId);

            //Assert
            _subject.ConversationMemberIds.Should().NotContain(_currentChatterId);
            _subject.ConversationMemberIds.Should().HaveCount(1);
            _subject.DomainEvents.Should().Contain(de => de is ConversationLeftEvent);
        }

        [Fact]
        public void PostMessage_AddsAMessageToConversation()
        {
            //Arrange
            _subject = CreateSampleConversationWithChatters(_currentChatterId);

            //Act
            _subject.PostMessage(_currentChatterId, "Text");

            //Assert
            _subject.TotalMessageCount.Should().Be(1);
            _subject.LoadedMessages.Should().HaveCount(1);
            _subject.LoadedMessages.Single().Text.Should().Be("Text");
            _subject.DomainEvents.Should().Contain(de => de is MessagePostedEvent);
        }

        [Fact]
        public void DeleteMessage_GivenExistingConversationAndMessage_RemovesMessage()
        {
            //Arrange
            _subject = CreateSampleConversationWithChatters(_currentChatterId);
            _subject.PostMessage(_currentChatterId, "Text");

            Message messageToDelete = _subject.LoadedMessages.Single();

            //Act
            _subject.DeleteMessage(_currentChatterId, messageToDelete.Id);

            //Assert
            _subject.LoadedMessages.Should().BeEmpty();
        }

        [Fact]
        public void DeleteMessage_GivenNonExistantMessage_ThrowsException()
        {
            //Arrange
            _subject = CreateSampleConversationWithChatters(_currentChatterId);
            _subject.PostMessage(_currentChatterId, "Text");

            //Act
            Action delete = () =>
            {
                _subject.DeleteMessage(_currentChatterId, new MessageId(Guid.Empty));
            };

            //Assert
            delete.Should().Throw<InvalidOperationException>();
            _subject.LoadedMessages.Should().HaveCount(1);
        }

        [Fact]
        public void DeleteMessage_WhenMessageDoesntBelongToCurrentUser_ThrowsException()
        {
            //Arrange
            _subject = CreateSampleConversationWithChatters(_currentChatterId);
            _subject.PostMessage(_chatterInConversationId, "Text");

            Message messageToDelete = _subject.LoadedMessages.Single();

            //Act
            Action delete = () =>
            {
                _subject.DeleteMessage(_currentChatterId, messageToDelete.Id);
            };

            //Assert
            delete.Should().Throw<InvalidOperationException>();
            _subject.LoadedMessages.Should().HaveCount(1);
        }

        [Fact]
        public void AddMemberToConversation_AddsChatterId_ToConversationMemberIds()
        {
            //Arrange
            _subject = CreateSampleConversationWithChatters(_currentChatterId);

            //Act
            Result result = _subject.AddMember(_currentChatterId, _chatterNotInConversationId);

            //Assert
            _subject.ConversationMemberIds.Should().Contain(_chatterNotInConversationId);
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void AddMemberToConversation_ReturnsFail_IfMemberAlreadyInConversation()
        {
            //Arrange
            _subject = CreateSampleConversationWithChatters(_currentChatterId);

            //Act
            Result result = _subject.AddMember(_currentChatterId, _chatterInConversationId);

            //Assert
            _subject.ConversationMemberIds.Should().HaveCount(2);
            result.IsFailed.Should().BeTrue();
        }

        [Fact]
        public void AddMemberToConversation_ReturnsFail_WhenCurrentChatterIsNotConversationOwner()
        {
            //Arrange
            _subject = CreateSampleConversationWithChatters(_chatterInConversationId);

            ChatterId chatOwnerId = _subject.OwnerChatterId;

            //Act
            Result result = _subject.AddMember(_currentChatterId, _chatterNotInConversationId);

            //Assert
            _currentChatterId.Should().NotBe(chatOwnerId);
            _subject.ConversationMemberIds.Should().HaveCount(2);
            result.IsFailed.Should().BeTrue();
        }

        [Fact]
        public void KickMemberFromConversation_ReturnsFail_WhenCurrentChatterIsNotConversationOwner()
        {
            //Arrange
            _subject = CreateSampleConversationWithChatters(_chatterNotInConversationId);

            ChatterId chatOwnerId = _subject.OwnerChatterId;

            //Act
            Result result = _subject.KickMember(_currentChatterId, _chatterInConversationId);

            //Assert
            _currentChatterId.Should().NotBe(chatOwnerId);
            result.IsFailed.Should().BeTrue();
            _subject.ConversationMemberIds.Should().Contain(_chatterInConversationId);
        }

        [Fact]
        public void KickMemberFromConversation_Throws_WhenMemberNotInConversation()
        {
            //Arrange
            ConversationId conversationId = new();
            _subject = CreateSampleConversationWithChatters(_currentChatterId);

            //Act
            Action kickMember = () =>
            {
                _subject.KickMember(_currentChatterId, _chatterNotInConversationId);
            };

            //Assert
            kickMember.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void KickMemberFromConversation_GivenConversationMember_RemovesThemFromList()
        {
            //Arrange
            _subject = CreateSampleConversationWithChatters(_currentChatterId);

            //Act
            _subject.KickMember(_currentChatterId, _chatterInConversationId);

            //Assert
            _subject.ConversationMemberIds.Should().NotContain(_chatterInConversationId);
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
