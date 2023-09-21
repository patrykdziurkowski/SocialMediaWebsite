using Application.Features.Shared;

namespace Application.Features.Chat.Events
{
    public class MessageDeletedEvent : DomainEvent
    {
        public MessageDeletedEvent(MessageId messageId)
        {
            MessageId = messageId;
        }
        public MessageId MessageId { get; set; }
    }
}
