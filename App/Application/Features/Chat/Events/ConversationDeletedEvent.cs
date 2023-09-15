using Application.Features.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Chat.Events
{
    public class ConversationDeletedEvent : DomainEvent
    {
        public ConversationDeletedEvent(int conversationId)
        {
            ConversationId = conversationId;
        }

        public int ConversationId { get; }
    }
}
