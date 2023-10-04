namespace Application.Features.Conversations.Dtos
{
    public class PostMessageModel
    {
        public string? Text { get; set; }
        public Guid? ReplyMessageId { get; set; }
    }
}
