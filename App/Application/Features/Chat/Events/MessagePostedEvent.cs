using Application.Features.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Chat.Events
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
