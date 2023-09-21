using Application.Features.Shared;

namespace Application.Features.Chat.Events
{
    public class ConversationDeletedEvent : DomainEvent
    {
        public ConversationDeletedEvent(ConversationId conversationId)
        {
            ConversationId = conversationId;
        }

        public ConversationId ConversationId { get; }
    }
}
