using Application.Features.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Chat.Events
{
    public class ConversationCreatedEvent : DomainEvent
    {
        public ConversationCreatedEvent(
            string title,
            string? description,
            int ownerUserId,
            List<int> conversationMemberIds)
        {
            Title = title;
            Description = description;
            OwnerUserId = ownerUserId;
            ConversationMemberIds = conversationMemberIds;
        }

        public string Title { get; set; }
        public string? Description { get; set; }
        public int OwnerUserId { get; set; }
        public List<int> ConversationMemberIds { get; set; }
    }
}
