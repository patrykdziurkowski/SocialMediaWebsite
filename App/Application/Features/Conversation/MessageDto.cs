using Application.Features.Conversations;

namespace Application.Features.Conversation
{
    public class MessageDto
    {
        public MessageDto(Message message)
        {
            Id = message.Id.Value;
            AuthorChatterId = message.AuthorChatterId.Value;
            Text = message.Text;
            MessageDateTime = message.MessageDateTime;
            ReplyMessageId = message.ReplyMessageId?.Value;
        }

        public Guid Id { get; private set; }
        public Guid AuthorChatterId { get; private set; }
        public string Text { get; set; }
        public DateTimeOffset MessageDateTime { get; private set; }
        public Guid? ReplyMessageId { get; private set; }
    }
}
