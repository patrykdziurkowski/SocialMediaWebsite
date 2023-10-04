using Application.Features.Conversations.Interfaces;
using Application.Features.Chatter;
using Application.Features.Shared;
using Dapper;
using System.Data;

namespace Application.Features.Conversations
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IConversationEventHandlerFactory _eventHandlerFactory;

        public ConversationRepository(
            IConnectionFactory connectionFactory,
            IConversationEventHandlerFactory eventHandlerFactory)
        {
            _connectionFactory = connectionFactory;
            _eventHandlerFactory = eventHandlerFactory;
        }

        public async Task<IEnumerable<Conversation>> GetAllAsync(ChatterId currentChatterId)
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
                        new
                        {
                            UserId = currentChatterId
                        });

            foreach (Conversation conversation in conversations)
            {
                await LoadConversationMessagesAsync(conversation, connection);
                await LoadConversationMembersAsync(conversation, connection);
            }

            return conversations;
        }

        public async Task<Conversation> GetByIdAsync(ChatterId currentChatterId, ConversationId conversationId)
        {
            using IDbConnection connection = _connectionFactory.GetConnection(ConnectionType.SqlConnection);
            connection.Open();

            Conversation conversation = await connection
                    .QuerySingleAsync<Conversation>(
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
                        WHERE UserId = @UserId
                        AND ConversationId = @ConversationId)
                        """,
                        new
                        {
                            UserId = currentChatterId,
                            ConversationId = conversationId
                        });

            await LoadConversationMessagesAsync(conversation, connection);
            await LoadConversationMembersAsync(conversation, connection);

            return conversation;
        }

        public async Task SaveAsync(Conversation conversation)
        {
            using IDbConnection connection = _connectionFactory.GetConnection(ConnectionType.SqlConnection);
            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();

            foreach (DomainEvent domainEvent in conversation.DomainEvents)
            {
                IEventHandler handler = _eventHandlerFactory.GetHandler(domainEvent);
                await handler.Handle(domainEvent, connection, transaction);
            }

            transaction.Commit();
        }

        private async Task LoadConversationMembersAsync(Conversation conversation, IDbConnection connection)
        {
            IEnumerable<ChatterId> conversationMemberIds = await connection
                .QueryAsync<ChatterId>(
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
