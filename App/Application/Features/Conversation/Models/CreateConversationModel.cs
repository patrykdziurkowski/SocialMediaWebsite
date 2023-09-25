namespace Application.Features.Chat.Dtos
{
    public class CreateConversationModel
    {
        public List<Guid>? ConversationMemberIds { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
