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
            Guid messageId,
            ChatterId authorUserId,
            string text,
            Guid conversationId,
            Guid? replyMessageId)
        {
            MessageId = messageId;
            AuthorUserId = authorUserId;
            Text = text;
            ConversationId = conversationId;
            ReplyMessageId = replyMessageId;
        }

        public Guid MessageId { get; set; }
        public ChatterId AuthorUserId { get; set; }
        public string Text { get; set; }
        public Guid ConversationId { get; set; }
        public Guid? ReplyMessageId { get; set;}
    }
}
