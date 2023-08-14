using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Chat
{
    public class Conversation
    {
        public Conversation(
            int id,
            DateTimeOffset creationDateTime,
            int messageCount,
            int ownerChatterId,
            List<Message> messages,
            List<Chatter> conversationMembers,
            string title,
            string? description = null)
        {
            Id = id;
            CreationDateTime = creationDateTime;
            TotalMessageCount = messageCount;
            OwnerChatterId = ownerChatterId;
            LoadedMessages = messages;
            ConversationMembers = conversationMembers;
            Title = title;
            Description = description;
        }

        public Conversation(
            int ownerChatterId,
            List<Chatter> conversationMembers,
            string title,
            string? description = null)
        {
            Id = null;
            CreationDateTime = null;
            TotalMessageCount = 0;
            OwnerChatterId = ownerChatterId;
            LoadedMessages = new List<Message>();
            ConversationMembers = conversationMembers;
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
        public List<Chatter> ConversationMembers { get; set; }
    }
}
