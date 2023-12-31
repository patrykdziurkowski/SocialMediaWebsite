﻿using Application.Features.Shared;
using Dapper;
using System.Data;

namespace Application.Features.Conversations.EventHandlers
{
    public class ConversationDeletedHandler : IEventHandler
    {
        public async Task Handle(
            DomainEvent domainEvent,
            IDbConnection connection,
            IDbTransaction transaction)
        {
            await connection.ExecuteAsync(
                $"""
                DELETE FROM SocialMediaWebsite.dbo.ConversationUsers
                WHERE ConversationId = @ConversationId
                """,
                domainEvent,
                transaction);

            await connection.ExecuteAsync(
                $"""
                DELETE FROM SocialMediaWebsite.dbo.MessageLikes
                WHERE LikedMessageId IN 
                (SELECT Id
                FROM SocialMediaWebsite.dbo.Messages
                WHERE ConversationId = @ConversationId)
                """,
                domainEvent,
                transaction);

            await connection.ExecuteAsync(
                $"""
                DELETE FROM SocialMediaWebsite.dbo.Messages
                WHERE ConversationId = @ConversationId
                """,
                domainEvent,
                transaction);

            await connection.ExecuteAsync(
                $"""
                DELETE FROM SocialMediaWebsite.dbo.Conversations
                WHERE Id = @ConversationId
                """,
                domainEvent,
                transaction);

        }
    }
}
