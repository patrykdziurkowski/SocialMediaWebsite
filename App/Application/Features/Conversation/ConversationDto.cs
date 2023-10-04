using Application.Features.Chatter;

namespace Application.Features.Conversations
{
    public class ConversationDto
    {
        public ConversationDto(Conversation conversation)
        {
            Id = conversation.Id.Value;
            Title = conversation.Title;
            Description = conversation.Description;
            CreationDateTime = conversation.CreationDateTime;
            TotalMessageCount = conversation.TotalMessageCount;
            OwnerChatterId = conversation.OwnerChatterId.Value;
            LoadedMessages = conversation.LoadedMessages.ToList();
            ConversationMemberIds = conversation.ConversationMemberIds.ToList();
        }

        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public DateTimeOffset CreationDateTime { get; private set; }
        public int TotalMessageCount { get; private set; }
        public Guid OwnerChatterId { get; private set; }
        public IEnumerable<Message> LoadedMessages { get; private set; }
        public IEnumerable<ChatterId> ConversationMemberIds { get; private set; }

    }
}
