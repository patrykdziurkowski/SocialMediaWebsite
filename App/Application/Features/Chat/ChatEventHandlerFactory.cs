﻿using Application.Features.Chat.EventHandlers;
using Application.Features.Chat.Events;
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
    public class ChatEventHandlerFactory : IChatEventHandlerFactory
    {
        public IEventHandler GetHandler(DomainEvent domainEvent)
        {
            return domainEvent switch
            {
                ConversationLeftEvent => new ConversationLeftHandler(),
                ConversationCreatedEvent => new ConversationCreatedHandler(),
                MessagePostedEvent => new MessagePostedHandler(),
                MessageDeletedEvent => new MessageDeletedHandler(),
                _ => throw new NotImplementedException($"No handler found for provided event {domainEvent.GetType()}"),
            };
        }
    }
}
