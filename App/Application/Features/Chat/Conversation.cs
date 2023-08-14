using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Chat
{
    public class Conversation
    {
        private readonly List<Message> _loadedMessages;
        private readonly List<Chatter> _conversationMembers;

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
            MessageCount = messageCount;
            OwnerChatterId = ownerChatterId;
            _loadedMessages = messages;
            _conversationMembers = conversationMembers;
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
            MessageCount = 0;
            OwnerChatterId = ownerChatterId;
            _loadedMessages = new List<Message>();
            _conversationMembers = conversationMembers;
            Title = title;
            Description = description;
        }

        public int? Id { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public DateTimeOffset? CreationDateTime { get; private set; }
        public int MessageCount { get; private set; }
        public int OwnerChatterId { get; private set; }
        public IEnumerable<Message> LoadedMessages => _loadedMessages;
        public IEnumerable<Chatter> ConversationMembers => _conversationMembers;
    }
}
