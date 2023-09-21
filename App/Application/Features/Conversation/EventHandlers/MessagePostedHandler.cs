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
    public class MessagePostedHandler : IEventHandler
    {
        public async Task Handle(
            DomainEvent domainEvent,
            IDbConnection connection,
            IDbTransaction transaction)
        {
            await connection.ExecuteAsync(
                $"""
                INSERT INTO SocialMediaWebsite.dbo.Messages
                (Id, AuthorUserId, Text, ConversationId, ReplyMessageId)
                VALUES
                (@MessageId, @AuthorUserId, @Text, @ConversationId, @ReplyMessageId)
                """,
                domainEvent,
                transaction);
        }
    }
}
