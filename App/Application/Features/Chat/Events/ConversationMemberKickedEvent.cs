﻿using Application.Features.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Chat.Events
{
    public class ConversationMemberKickedEvent : DomainEvent
    {
        public ConversationMemberKickedEvent(
            Guid conversationId,
            Guid chatterId)
        {
            ConversationId = conversationId;
            ChatterId = chatterId;
        }

        public Guid ConversationId { get; }
        public Guid ChatterId { get; }
    }
}
