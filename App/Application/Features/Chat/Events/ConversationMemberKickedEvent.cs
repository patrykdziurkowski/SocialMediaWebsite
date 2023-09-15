using Application.Features.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Chat.Events
{
    public class ConversationMemberKickedEvent : DomainEvent
    {
        public ConversationMemberKickedEvent(
            int conversationId,
            int chatterId)
        {
            ConversationId = conversationId;
            ChatterId = chatterId;
        }

        public int ConversationId { get; }
        public int ChatterId { get; }
    }
}
