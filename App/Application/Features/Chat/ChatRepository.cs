using Application.Features.Chat.Events;
using Application.Features.Chat.Interfaces;
using Application.Features.Shared;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Chat
{
    public class ChatRepository : IChatRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IChatEventHandlerFactory _eventHandlerFactory;

        public ChatRepository(
            IConnectionFactory connectionFactory,
            IChatEventHandlerFactory eventHandlerFactory)
        {
            _connectionFactory = connectionFactory;
            _eventHandlerFactory = eventHandlerFactory;
        }
        public async Task<Chat> GetAsync(int userId)
        {
            using IDbConnection connection = _connectionFactory.GetConnection(ConnectionType.SqlConnection);
            connection.Open();

            IEnumerable<Conversation> conversations = await connection
                            .QueryAsync<Conversation>(
                                $"""
                                SELECT
                                    Id AS {nameof(Conversation.Id)},
                                    Title AS {nameof(Conversation.Title)},
                                    Description AS {nameof(Conversation.Description)},
                                    CreationDateTime AS {nameof(Conversation.CreationDateTime)},
                                    (SELECT COUNT(*) FROM SocialMediaWebsite.dbo.Messages WHERE ConversationId = Conversations.Id) AS {nameof(Conversation.TotalMessageCount)},
                                    OwnerUserId AS {nameof(Conversation.OwnerChatterId)}
                                FROM SocialMediaWebsite.dbo.Conversations
                                WHERE Id IN
                                (SELECT ConversationId
                                FROM SocialMediaWebsite.dbo.ConversationUsers
                                WHERE UserId = @UserId)
                                """,
                                new { UserId = userId });

            foreach (Conversation conversation in conversations)
            {
                await LoadConversationMessagesAsync(conversation, connection);
                await LoadConversationMembersAsync(conversation, connection);
            }

            return new Chat(userId, conversations.ToList());
        }

        public async Task SaveAsync(Chat chat)
        {
            using IDbConnection connection = _connectionFactory.GetConnection(ConnectionType.SqlConnection);
            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();

            foreach (DomainEvent domainEvent in chat.DomainEvents)
            {
                IEventHandler handler = _eventHandlerFactory.GetHandler(domainEvent);
                await handler.Handle(domainEvent, connection, transaction);
            }

            transaction.Commit();
        }

        private async Task LoadConversationMembersAsync(Conversation conversation, IDbConnection connection)
        {
            IEnumerable<int> conversationMemberIds = await connection
                    .QueryAsync<int>(
                        $"""
                        SELECT UserId
                        FROM SocialMediaWebsite.dbo.ConversationUsers
                        WHERE ConversationId = @ConversationId
                        """,
                        new { ConversationId = conversation.Id });

            conversation.ConversationMemberIds = conversationMemberIds.ToList();
        }

        private async Task LoadConversationMessagesAsync(Conversation conversation, IDbConnection connection)
        {
            IEnumerable<Message> conversationMessages = await connection
                .QueryAsync<Message>(
                    $"""
                    SELECT 
                        Id AS {nameof(Message.Id)},
                        AuthorUserId AS {nameof(Message.AuthorChatterId)},
                        Text AS {nameof(Message.Text)},
                        MessageDateTime AS {nameof(Message.MessageDateTime)},
                        ReplyMessageId AS {nameof(Message.ReplyMessageId)}
                    FROM SocialMediaWebsite.dbo.Messages
                    WHERE ConversationId = @ConversationId
                    """,
                    new { ConversationId = conversation.Id });

            conversation.LoadedMessages = conversationMessages.ToList();
        }

    }
}
