using Application.Features.Chat.Events;
using Application.Features.Chat;
using FluentAssertions;
using Xunit;
using Tests.UnitTests.Fakes;
using Application.Features.Chatter;

namespace Tests.UnitTests.Domain
{
    public class AggreggateRootTests
    {
        private readonly AggreggateRootChild _subject;

        public AggreggateRootTests()
        {
            _subject = new AggreggateRootChild();
        }

        [Fact]
        public void RaiseDomainEvent_AddsEventToList()
        {
            //Arrange
            ConversationLeftEvent e = new(new ConversationId(), new ChatterId());

            //Act
            _subject.RaiseDomainEvent(e);

            //Assert
            _subject.DomainEvents.Should().HaveCount(1);
        }

        [Fact]
        public void ClearDomainEvents_RemovesAllEventsFromList()
        {
            //Arrange
            ConversationLeftEvent event1 = new(new ConversationId(), new ChatterId());
            ConversationLeftEvent event2 = new(new ConversationId(), new ChatterId());

            _subject.RaiseDomainEvent(event1);
            _subject.RaiseDomainEvent(event2);

            int numberOfEventsBeforeClearing = _subject.DomainEvents.Count();

            //Act
            _subject.ClearDomainEvents();

            //Assert
            numberOfEventsBeforeClearing.Should().Be(2);
            _subject.DomainEvents.Should().BeEmpty();
        }
    }
}
