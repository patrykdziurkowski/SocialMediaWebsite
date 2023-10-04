using Application.Features.Chatter;
using Application.Features.Shared;

namespace Application.Features.Conversations.Events
{
    public class ConversationMemberKickedEvent : DomainEvent
    {
        public ConversationMemberKickedEvent(
            ConversationId conversationId,
            ChatterId chatterId)
        {
            ConversationId = conversationId;
            ChatterId = chatterId;
        }

        public ConversationId ConversationId { get; }
        public ChatterId ChatterId { get; }
    }
}
