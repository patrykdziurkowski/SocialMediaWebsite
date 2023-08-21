using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Chat
{
    public class Message
    {
        private Message()
        {
            Id = default!;
            AuthorChatterId = default!;
            Text = default!;
            MessageDateTime = default!;
            ReplyMessageId = default;
        }
        public Message(
            int authorChatterId,
            string text,
            int? replyMessageId = null)
        {
            AuthorChatterId = authorChatterId;
            Text = text;
            ReplyMessageId = replyMessageId;
        }

        public Message(
            int id,
            int authorChatterId,
            string text,
            DateTimeOffset messageDateTime,
            int? replyMessageId = null)
        {
            Id = id;
            AuthorChatterId = authorChatterId;
            Text = text;
            MessageDateTime = messageDateTime;
            ReplyMessageId = replyMessageId;
        }

        public int Id { get; private set; }
        public int AuthorChatterId { get; private set; }
        public string Text { get; set; }
        public DateTimeOffset MessageDateTime { get; private set; }
        public int? ReplyMessageId { get; private set; }
    }
}
