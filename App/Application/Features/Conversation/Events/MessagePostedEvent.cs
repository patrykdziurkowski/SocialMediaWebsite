using Application.Features.Chatter;
using Application.Features.Shared;

namespace Application.Features.Conversations.Events
{
    public class MessagePostedEvent : DomainEvent
    {
        public MessagePostedEvent(
            MessageId messageId,
            ChatterId authorUserId,
            string text,
            ConversationId conversationId,
            MessageId? replyMessageId)
        {
            MessageId = messageId;
            AuthorUserId = authorUserId;
            Text = text;
            ConversationId = conversationId;
            ReplyMessageId = replyMessageId;
        }

        public MessageId MessageId { get; set; }
        public ChatterId AuthorUserId { get; set; }
        public string Text { get; set; }
        public ConversationId ConversationId { get; set; }
        public MessageId? ReplyMessageId { get; set;}
    }
}
