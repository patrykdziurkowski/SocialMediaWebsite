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

            List<Conversation> conversation = new()
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

            _subject = new(LeavingChatterId, conversation);
            IEnumerable<Chatter> conversationMembers = _subject.Conversations.Single().ConversationMembers;

            //Act
            _subject.LeaveConversation(ConversationId);

            //Assert
            conversationMembers.Should().NotContain(chatter => chatter.Id == LeavingChatterId);
            conversationMembers.Should().HaveCount(2);
            conversation.Should().BeEmpty();
        }

        [Fact]
        public void LeaveConversation_DoesNotRemoveChatterFromConversation_WhenInvalidId()
        {
            //Arrange
            const int LeavingChatterId = 1;

            List<Conversation> conversation = new()
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

            _subject = new(LeavingChatterId, conversation);
            IEnumerable<Chatter> conversationMembers = _subject.Conversations.Single().ConversationMembers;

            //Act
            _subject.LeaveConversation(999);

            //Assert
            conversationMembers.Should().Contain(chatter => chatter.Id == LeavingChatterId);
            conversationMembers.Should().HaveCount(3);
            conversation.Should().NotBeEmpty();
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
