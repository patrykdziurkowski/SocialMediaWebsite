using Application.Features.Chat.Events;
using Application.Features.Chatter;
using Application.Features.Shared;
using Dapper;
using System.Data;

namespace Application.Features.Chat.EventHandlers
{
    public class ConversationStartedHandler : IEventHandler
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

            foreach (ChatterId chatterId in ((ConversationStartedEvent) domainEvent).ConversationMemberIds)
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
                        ConversationId = ((ConversationStartedEvent) domainEvent).ConversationId
                    },
                    transaction);
            }
        }
    }
}
