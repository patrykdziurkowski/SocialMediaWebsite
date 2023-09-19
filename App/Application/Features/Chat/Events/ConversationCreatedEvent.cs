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
            Guid conversationId,
            string title,
            string? description,
            Guid ownerUserId,
            List<Guid> conversationMemberIds)
        {
            ConversationId = conversationId;
            Title = title;
            Description = description;
            OwnerUserId = ownerUserId;
            ConversationMemberIds = conversationMemberIds;
        }

        public Guid ConversationId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public Guid OwnerUserId { get; set; }
        public List<Guid> ConversationMemberIds { get; set; }
    }
}
