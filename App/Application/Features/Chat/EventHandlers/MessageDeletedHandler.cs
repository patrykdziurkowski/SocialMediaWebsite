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
    public class MessageDeletedHandler : IEventHandler
    {
        public async Task Handle(DomainEvent domainEvent, IDbConnection connection, IDbTransaction transaction)
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
