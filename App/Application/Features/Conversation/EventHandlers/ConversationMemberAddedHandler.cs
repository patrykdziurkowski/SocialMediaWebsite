using Application.Features.Chat.Events;
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
            ConversationMemberAddedEvent memberAddedEvent = (ConversationMemberAddedEvent) domainEvent;

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
                    ChatterId = memberAddedEvent.ChatterId,
                    ConversationId = memberAddedEvent.ConversationId
                },
                transaction);
        }
    }
}
