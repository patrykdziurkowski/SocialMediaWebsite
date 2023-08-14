using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Chat
{
    public class Message
    {
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
        public string Text { get; private set; }
        public DateTimeOffset MessageDateTime { get; private set; }
        public int? ReplyMessageId { get; private set; }
    }
}
