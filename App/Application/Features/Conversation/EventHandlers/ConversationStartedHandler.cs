using Application.Features.Conversations.Events;
using Application.Features.Chatter;
using Application.Features.Shared;
using Dapper;
using System.Data;

namespace Application.Features.Conversations.EventHandlers
{
    public class ConversationStartedHandler : IEventHandler
    {
        public async Task Handle(
            DomainEvent domainEvent,
            IDbConnection connection,
            IDbTransaction transaction)
        {
            ConversationStartedEvent conversationStartedEvent = (ConversationStartedEvent) domainEvent;

            await connection.ExecuteAsync(
                $"""
                INSERT INTO SocialMediaWebsite.dbo.Conversations
                (Id, Title, Description, OwnerUserId)
                VALUES
                (@ConversationId, @Title, @Description, @OwnerUserId)
                """,
                domainEvent,
                transaction);

            foreach (ChatterId chatterId in conversationStartedEvent.ConversationMemberIds)
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
                        ConversationId = conversationStartedEvent.ConversationId
                    },
                    transaction);
            }
        }
    }
}
