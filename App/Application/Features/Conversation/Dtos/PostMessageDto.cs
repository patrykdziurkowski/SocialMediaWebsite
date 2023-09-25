namespace Application.Features.Conversation.Dtos
{
    public class PostMessageDto
    {
        public string? Text { get; set; }
        public Guid? ReplyMessageId { get; set; }
    }
}
