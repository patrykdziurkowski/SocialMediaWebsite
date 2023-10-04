using Application.Features.Shared;
using Dapper;
using System.Data;

namespace Application.Features.Conversations.EventHandlers
{
    public class ConversationLeftHandler : IEventHandler
    {
        public async Task Handle(
            DomainEvent domainEvent,
            IDbConnection connection,
            IDbTransaction transaction)
        {
            await connection.ExecuteAsync(
                $"""
                DELETE FROM SocialMediaWebsite.dbo.ConversationUsers
                WHERE
                    ConversationId = @ConversationId
                    AND UserId = @ChatterId
                """,
                domainEvent,
                transaction);
        }
    }
}
