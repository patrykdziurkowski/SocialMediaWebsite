using Application.Features.Conversations.EventHandlers;
using Application.Features.Conversations.Events;
using Application.Features.Conversations.Interfaces;
using Application.Features.Shared;

namespace Application.Features.Conversations
{
    public class ConversationEventHandlerFactory : IConversationEventHandlerFactory
    {
        public IEventHandler GetHandler(DomainEvent domainEvent)
        {
            return domainEvent switch
            {
                ConversationLeftEvent => new ConversationLeftHandler(),
                ConversationStartedEvent => new ConversationStartedHandler(),
                MessagePostedEvent => new MessagePostedHandler(),
                MessageDeletedEvent => new MessageDeletedHandler(),
                ConversationMemberAddedEvent => new ConversationMemberAddedHandler(),
                ConversationMemberKickedEvent => new ConversationMemberKickedHandler(),
                ConversationDeletedEvent => new ConversationDeletedHandler(),
                _ => throw new NotImplementedException($"No handler found for provided event {domainEvent.GetType()}"),
            };
        }
    }
}
