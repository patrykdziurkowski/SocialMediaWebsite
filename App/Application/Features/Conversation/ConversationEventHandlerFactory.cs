using Application.Features.Chat.EventHandlers;
using Application.Features.Chat.Events;
using Application.Features.Chat.Interfaces;
using Application.Features.Shared;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Application.Features.Chat
{
    public class ConversationEventHandlerFactory : IConversationEventHandlerFactory
    {
        public IEventHandler GetHandler(DomainEvent domainEvent)
        {
            return domainEvent switch
            {
                ConversationLeftEvent => new ConversationLeftHandler(),
                ConversationCreatedEvent => new ConversationCreatedHandler(),
                MessagePostedEvent => new MessagePostedHandler(),
                MessageDeletedEvent => new MessageDeletedHandler(),
                ConversationMemberAddedEvent => new ConversationMemberAddedHandler(),
                ConversationMemberKickedEvent => new ConversationMemberKickedHandler(),
                ConversationDeletedEvent => new ConversationDeletedHandler(),
                _ => throw new NotImplementedException($"No handler found for provided event {domainEvent.GetType()}"),
            };
        }
    }
}
