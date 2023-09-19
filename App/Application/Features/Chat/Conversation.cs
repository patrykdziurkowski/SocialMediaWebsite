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
            ConversationMemberIds = new List<Guid>();
        }

        public Conversation(
            DateTimeOffset creationDateTime,
            Guid ownerChatterId,
            List<Guid> conversationMemberIds,
            string title,
            string? description = null)
        {
            Id = Guid.NewGuid();
            CreationDateTime = creationDateTime;
            TotalMessageCount = 0;
            OwnerChatterId = ownerChatterId;
            LoadedMessages = new List<Message>();
            ConversationMemberIds = conversationMemberIds;
            Title = title;
            Description = description;
        }

        public Guid Id { get; private set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset? CreationDateTime { get; private set; }
        public int TotalMessageCount { get; set; }
        public Guid OwnerChatterId { get; set; }
        public List<Message> LoadedMessages { get; set; }
        public List<Guid> ConversationMemberIds { get; set; }
    }
}
