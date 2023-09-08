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
            int conversationId,
            int userId)
        {
            ConversationId = conversationId;
            UserId = userId;
        }

        public int ConversationId { get; set; }
        public int UserId { get; set; }
    }
}
