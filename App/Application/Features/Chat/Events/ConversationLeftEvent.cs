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
            Guid conversationId,
            Guid userId)
        {
            ConversationId = conversationId;
            UserId = userId;
        }

        public Guid ConversationId { get; set; }
        public Guid UserId { get; set; }
    }
}
