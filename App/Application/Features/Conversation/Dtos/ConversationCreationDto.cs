namespace Application.Features.Chat.Dtos
{
    public class ConversationCreationDto
    {
        public List<Guid>? ConversationMemberIds { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
