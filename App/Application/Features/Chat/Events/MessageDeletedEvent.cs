﻿using Application.Features.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Chat.Events
{
    public class MessageDeletedEvent : DomainEvent
    {
        public MessageDeletedEvent(MessageId messageId)
        {
            MessageId = messageId;
        }
        public MessageId MessageId { get; set; }
    }
}
