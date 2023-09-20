using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Chat
{
    public class Conversation
    {
        private Conversation()
        {
            Id = default!;
            Title = default!;
            Description = default!;
            CreationDateTime = default!;
            TotalMessageCount = default!;
            OwnerChatterId = default!;
            LoadedMessages = new List<Message>();
            ConversationMemberIds = new List<ChatterId>();
        }

        public Conversation(
            DateTimeOffset creationDateTime,
            ChatterId ownerChatterId,
            List<ChatterId> conversationMemberIds,
            string title,
            string? description = null)
        {
            Id = new ConversationId(Guid.NewGuid());
            CreationDateTime = creationDateTime;
            TotalMessageCount = 0;
            OwnerChatterId = ownerChatterId;
            LoadedMessages = new List<Message>();
            ConversationMemberIds = conversationMemberIds;
            Title = title;
            Description = description;
        }

        public ConversationId Id { get; private set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset CreationDateTime { get; private set; }
        public int TotalMessageCount { get; set; }
        public ChatterId OwnerChatterId { get; set; }
        public List<Message> LoadedMessages { get; set; }
        public List<ChatterId> ConversationMemberIds { get; set; }
    }
}
