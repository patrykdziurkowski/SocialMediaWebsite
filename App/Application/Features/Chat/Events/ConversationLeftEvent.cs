using Application.Features.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Chat.Events
{
    public class ConversationLeftEvent : DomainEvent
    {
        public ConversationLeftEvent(
            ConversationId conversationId,
            ChatterId userId)
        {
            ConversationId = conversationId;
            UserId = userId;
        }

        public ConversationId ConversationId { get; set; }
        public ChatterId UserId { get; set; }
    }
}
