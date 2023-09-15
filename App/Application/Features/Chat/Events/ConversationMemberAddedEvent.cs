using Application.Features.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Chat.Events
{
    public class ConversationMemberAddedEvent : DomainEvent
    {
        public ConversationMemberAddedEvent(
            int conversationId,
            int chatterId)
        {
            ConversationId = conversationId;
            ChatterId = chatterId;
        }

        public int ConversationId { get; set; }
        public int ChatterId { get; set; }
    }
}
