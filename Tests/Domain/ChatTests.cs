using Application.Features.Chat;
using FluentAssertions;
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
            _subject.CreateConversation(new List<Chatter>(), "Title", "Description");

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
            List<Chatter> conversationMembers = GetThreeSampleConversationMembers();

            //Act
            _subject.CreateConversation(conversationMembers, "Title", "Description");

            //Assert
            _subject.Conversations.Single().ConversationMembers.Should().HaveCount(conversationMembers.Count);

        }

        [Fact]
        public void LeaveConversation_RemovesChatterFromConversation_WhenValidId()
        {
            //Arrange
            const int ConversationId = 50;
            const int LeavingChatterId = 1;

            List<Conversation> conversations = new()
            {
                new Conversation(
                    ConversationId,
                    new DateTimeOffset(),
                    0,
                    2,
                    new List<Message>(),
                    GetThreeSampleConversationMembers(),
                    "Title")
            }; 

            _subject = new(LeavingChatterId, conversations);
            IEnumerable<Chatter> conversationMembers = _subject.Conversations.Single().ConversationMembers;

            //Act
            _subject.LeaveConversation(ConversationId);

            //Assert
            conversationMembers.Should().NotContain(chatter => chatter.Id == LeavingChatterId);
            conversationMembers.Should().HaveCount(2);
            conversations.Should().BeEmpty();
        }

        [Fact]
        public void LeaveConversation_DoesNotRemoveChatterFromConversation_WhenInvalidId()
        {
            //Arrange
            const int LeavingChatterId = 1;
            List<Conversation> conversations = new()
            {
                new Conversation(
                    50,
                    new DateTimeOffset(),
                    0,
                    2,
                    new List<Message>(),
                    GetThreeSampleConversationMembers(),
                    "Title")
            };

            _subject = new(LeavingChatterId, conversations);
            IEnumerable<Chatter> conversationMembers = _subject.Conversations.Single().ConversationMembers;

            //Act
            _subject.LeaveConversation(999);

            //Assert
            conversationMembers.Should().Contain(chatter => chatter.Id == LeavingChatterId);
            conversationMembers.Should().HaveCount(3);
            conversations.Should().NotBeEmpty();
        }

        [Fact]
        public void PostMessage_GivenExistingConversationId_AddsAMessageToConversation()
        {
            //Arrange
            const int CurrentChatterId = 1;
            List<Conversation> conversations = new()
            {
                new Conversation(
                    50,
                    new DateTimeOffset(),
                    0,
                    2,
                    new List<Message>(),
                    GetThreeSampleConversationMembers(),
                    "Title")
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
        public void PostMessage_GivenNonExistantConversationId_DoesntAddAnyMessage()
        {
            //Arrange
            const int CurrentChatterId = 1;
            List<Conversation> conversations = new()
            {
                new Conversation(
                    50,
                    new DateTimeOffset(),
                    0,
                    2,
                    new List<Message>(),
                    GetThreeSampleConversationMembers(),
                    "Title")
            };

            _subject = new(CurrentChatterId, conversations);
            Conversation conversation = _subject.Conversations.Single(c => c.Id == 50);

            //Act
            _subject.PostMessage(999, "Text");

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
                new Conversation(
                    50,
                    new DateTimeOffset(),
                    0,
                    2,
                    new List<Message>(),
                    GetThreeSampleConversationMembers(),
                    "Title")
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
        public void DeleteMessage_GivenNonExistantConversation_DoesntRemoveMessage()
        {
            //Arrange
            const int CurrentChatterId = 1;
            List<Conversation> conversations = new()
            {
                new Conversation(
                    50,
                    new DateTimeOffset(),
                    0,
                    2,
                    new List<Message>(),
                    GetThreeSampleConversationMembers(),
                    "Title")
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
            _subject.DeleteMessage(70, 600);

            //Assert
            conversation.LoadedMessages.Should().HaveCount(1);
        }

        [Fact]
        public void DeleteMessage_GivenNonExistantMessage_DoesntRemoveMessage()
        {
            //Arrange
            const int CurrentChatterId = 1;
            List<Conversation> conversations = new()
            {
                new Conversation(
                    50,
                    new DateTimeOffset(),
                    0,
                    2,
                    new List<Message>(),
                    GetThreeSampleConversationMembers(),
                    "Title")
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
            _subject.DeleteMessage(50, 700);

            //Assert
            conversation.LoadedMessages.Should().HaveCount(1);
        }


        private static List<Chatter> GetThreeSampleConversationMembers()
        {
            return new List<Chatter>()
            {
                new Chatter(
                    1,
                    "John",
                    new DateTimeOffset()),
                new Chatter(
                    2,
                    "Brian",
                    new DateTimeOffset()),
                new Chatter(
                    3,
                    "Edward",
                    new DateTimeOffset()),
            };
        }

    }
}
