using Application.Features.Chatter;
using Application.Features.Shared;

namespace Application.Features.Conversations.Events
{
    public class ConversationMemberAddedEvent : DomainEvent
    {
        public ConversationMemberAddedEvent(
            ConversationId conversationId,
            ChatterId chatterId)
        {
            ConversationId = conversationId;
            ChatterId = chatterId;
        }
        public ConversationId ConversationId { get; set; }
        public ChatterId ChatterId { get; set; }
    }
}
