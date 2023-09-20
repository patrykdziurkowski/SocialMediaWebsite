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
            ConversationId conversationId,
            ChatterId chatterId)
        {
            ConversationId = conversationId;
            ChatterId = chatterId;
        }
        public ConversationId ConversationId { get; set; }
        public ChatterId ChatterId { get; set; }
    }
}
