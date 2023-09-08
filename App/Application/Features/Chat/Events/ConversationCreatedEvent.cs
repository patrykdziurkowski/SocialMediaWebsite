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
            List<Chatter> conversationMembers)
        {
            Title = title;
            Description = description;
            OwnerUserId = ownerUserId;
            ConversationMembers = conversationMembers;
        }

        public string Title { get; set; }
        public string? Description { get; set; }
        public int OwnerUserId { get; set; }
        public List<Chatter> ConversationMembers { get; set; }
    }
}
