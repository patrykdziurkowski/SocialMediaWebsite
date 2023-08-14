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
            List<Chatter> conversationMembers = new()
            {
                new Chatter(
                    1,
                    "John",
                    new DateTimeOffset()),
                new Chatter(
                    2,
                    "Brian",
                    new DateTimeOffset()),
            };

            //Act
            _subject.CreateConversation(conversationMembers, "Title", "Description");

            //Assert
            _subject.Conversations.Single().ConversationMembers.Should().HaveCount(2);

        }

    }
}
