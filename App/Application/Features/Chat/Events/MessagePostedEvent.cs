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
            int authorUserId,
            string text,
            int conversationId,
            int? replyMessageId)
        {
            AuthorUserId = authorUserId;
            Text = text;
            ConversationId = conversationId;
            ReplyMessageId = replyMessageId;
        }

        public int AuthorUserId { get; set; }
        public string Text { get; set; }
        public int ConversationId { get; set; }
        public int? ReplyMessageId { get; set;}
    }
}
