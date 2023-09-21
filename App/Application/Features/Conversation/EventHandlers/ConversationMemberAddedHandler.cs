﻿using Application.Features.Chat.Events;
using Application.Features.Shared;
using Dapper;
using System.Data;

namespace Application.Features.Chat.EventHandlers
{
    public class ConversationMemberAddedHandler : IEventHandler
    {
        public async Task Handle(
            DomainEvent domainEvent,
            IDbConnection connection,
            IDbTransaction transaction)
        {
            await connection.ExecuteAsync(
                $"""
                INSERT INTO SocialMediaWebsite.dbo.ConversationUsers
                (Id, UserId, ConversationId)
                VALUES
                (@LinkId, @ChatterId, @ConversationId)
                """,
                new
                {
                    LinkId = Guid.NewGuid(),
                    ChatterId = ((ConversationMemberAddedEvent)domainEvent).ChatterId,
                    ConversationId = ((ConversationMemberAddedEvent) domainEvent).ConversationId
                },
                transaction);
        }
    }
}