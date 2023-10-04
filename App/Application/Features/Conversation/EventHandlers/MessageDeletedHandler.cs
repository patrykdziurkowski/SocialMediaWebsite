using Application.Features.Shared;
using Dapper;
using System.Data;

namespace Application.Features.Conversations.EventHandlers
{
    public class MessageDeletedHandler : IEventHandler
    {
        public async Task Handle(
            DomainEvent domainEvent,
            IDbConnection connection,
            IDbTransaction transaction)
        {
            await connection.ExecuteAsync(
                $"""
                DELETE FROM SocialMediaWebsite.dbo.Messages
                WHERE Id = @MessageId
                """,
                domainEvent,
                transaction);
        }
    }
}
