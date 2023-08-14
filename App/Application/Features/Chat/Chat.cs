using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Chat
{
    public class Chat
    {
        private readonly List<Conversation> _conversations;

        public Chat(
            int chatterId,
            List<Conversation> conversations)
        {
            ChatterId = chatterId;
            _conversations = conversations;
        }

        public int ChatterId { get; private set; }
        public IEnumerable<Conversation> Conversations => _conversations;


        public void CreateConversation(
            List<Chatter> conversationMembers,
            string title,
            string? description = null)
        {
            Conversation newConversation = new(
                ChatterId,
                conversationMembers,
                title,
                description);

            _conversations.Add(newConversation);
        }


    }
}
