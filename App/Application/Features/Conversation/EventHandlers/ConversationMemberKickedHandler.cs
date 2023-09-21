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
    public class ConversationMemberKickedHandler : IEventHandler
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
