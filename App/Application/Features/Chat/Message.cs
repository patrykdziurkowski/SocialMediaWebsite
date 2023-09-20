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
            ChatterId authorChatterId,
            string text,
            DateTimeOffset messageDateTime,
            MessageId? replyMessageId = null)
        {
            Id = new MessageId();
            AuthorChatterId = authorChatterId;
            Text = text;
            MessageDateTime = messageDateTime;
            ReplyMessageId = replyMessageId;
        }

        public MessageId Id { get; private set; }
        public ChatterId AuthorChatterId { get; private set; }
        public string Text { get; set; }
        public DateTimeOffset MessageDateTime { get; private set; }
        public MessageId? ReplyMessageId { get; private set; }
    }
}
