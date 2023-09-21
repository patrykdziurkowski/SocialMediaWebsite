using Application.Features.Chat.Events;
using Application.Features.Chatter;
using Application.Features.Shared;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Chat.EventHandlers
{
    public class ConversationCreatedHandler : IEventHandler
    {
        public async Task Handle(
            DomainEvent domainEvent,
            IDbConnection connection,
            IDbTransaction transaction)
        {
            await connection.ExecuteAsync(
                $"""
                INSERT INTO SocialMediaWebsite.dbo.Conversations
                (Id, Title, Description, OwnerUserId)
                VALUES
                (@ConversationId, @Title, @Description, @OwnerUserId)
                """,
                domainEvent,
                transaction);

            foreach (ChatterId chatterId in ((ConversationCreatedEvent) domainEvent).ConversationMemberIds)
            {
                await connection.ExecuteAsync(
                    $"""
                    INSERT INTO SocialMediaWebsite.dbo.ConversationUsers
                    (Id, UserId, ConversationId)
                    VALUES
                    (@Id, @UserId, @ConversationId)
                    """,
                    new
                    {
                        Id = Guid.NewGuid(),
                        UserId = chatterId,
                        ConversationId = ((ConversationCreatedEvent) domainEvent).ConversationId
                    },
                    transaction);
            }
        }
    }
}
