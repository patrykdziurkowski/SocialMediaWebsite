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
            Guid authorChatterId,
            string text,
            DateTimeOffset messageDateTime,
            Guid? replyMessageId = null)
        {
            Id = Guid.NewGuid();
            AuthorChatterId = authorChatterId;
            Text = text;
            MessageDateTime = messageDateTime;
            ReplyMessageId = replyMessageId;
        }

        public Guid Id { get; private set; }
        public Guid AuthorChatterId { get; private set; }
        public string Text { get; set; }
        public DateTimeOffset MessageDateTime { get; private set; }
        public Guid? ReplyMessageId { get; private set; }
    }
}
