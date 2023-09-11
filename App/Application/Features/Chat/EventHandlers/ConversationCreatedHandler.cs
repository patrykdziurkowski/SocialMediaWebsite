using Application.Features.Chat.Events;
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
        public async Task Handle(DomainEvent domainEvent, IDbConnection connection, IDbTransaction transaction)
        {
            int insertedConversationId = await connection.QuerySingleAsync<int>(
                $"""
                INSERT INTO SocialMediaWebsite.dbo.Conversations
                (Title, Description, OwnerUserId)
                VALUES
                (@Title, @Description, @OwnerUserId);
                SELECT CAST(SCOPE_IDENTITY() as int)
                """,
                domainEvent,
                transaction);

            foreach (Chatter chatter in ((ConversationCreatedEvent) domainEvent).ConversationMembers)
            {
                await connection.ExecuteAsync(
                    $"""
                    INSERT INTO SocialMediaWebsite.dbo.ConversationUsers
                    (UserId, ConversationId)
                    VALUES
                    (@UserId, @ConversationId)
                    """,
                    new
                    {
                        UserId = chatter.Id,
                        ConversationId = insertedConversationId
                    },
                    transaction);
            }
        }
    }
}
