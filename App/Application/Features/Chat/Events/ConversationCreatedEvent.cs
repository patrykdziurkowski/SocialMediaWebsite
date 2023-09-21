using Application.Features.Chatter;
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
            ConversationId conversationId,
            string title,
            string? description,
            ChatterId ownerUserId,
            List<ChatterId> conversationMemberIds)
        {
            ConversationId = conversationId;
            Title = title;
            Description = description;
            OwnerUserId = ownerUserId;
            ConversationMemberIds = conversationMemberIds;
        }

        public ConversationId ConversationId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public ChatterId OwnerUserId { get; set; }
        public List<ChatterId> ConversationMemberIds { get; set; }
    }
}
