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
            Guid conversationId,
            Guid chatterId)
        {
            LinkId = Guid.NewGuid();
            ConversationId = conversationId;
            ChatterId = chatterId;
        }

        public Guid LinkId { get; set; }
        public Guid ConversationId { get; set; }
        public Guid ChatterId { get; set; }
    }
}
