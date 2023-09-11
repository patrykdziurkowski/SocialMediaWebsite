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
            ConversationMemberIds = new List<int>();
        }

        public Conversation(
            int id,
            DateTimeOffset creationDateTime,
            int messageCount,
            int ownerChatterId,
            List<Message> messages,
            List<int> conversationMemberIds,
            string title,
            string? description = null)
        {
            Id = id;
            CreationDateTime = creationDateTime;
            TotalMessageCount = messageCount;
            OwnerChatterId = ownerChatterId;
            LoadedMessages = messages;
            ConversationMemberIds = conversationMemberIds;
            Title = title;
            Description = description;
        }

        public Conversation(
            int ownerChatterId,
            List<int> conversationMemberIds,
            string title,
            string? description = null)
        {
            Id = null;
            CreationDateTime = null;
            TotalMessageCount = 0;
            OwnerChatterId = ownerChatterId;
            LoadedMessages = new List<Message>();
            ConversationMemberIds = conversationMemberIds;
            Title = title;
            Description = description;
        }

        public int? Id { get; private set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset? CreationDateTime { get; private set; }
        public int TotalMessageCount { get; set; }
        public int OwnerChatterId { get; set; }
        public List<Message> LoadedMessages { get; set; }
        public List<int> ConversationMemberIds { get; set; }
    }
}
