using Application.Features.Chat.Events;
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
    public class ChatRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        public ChatRepository(IConnectionFactory connectionFactory)
        {
           _connectionFactory = connectionFactory;
        }
        
        public async Task<Chat> GetAsync(int userId)
        {
            using IDbConnection connection = _connectionFactory.GetConnection(ConnectionType.SqlConnection);
            connection.Open();

            IEnumerable<Conversation> conversations = await connection
                            .QueryAsync<Conversation>(
                                "SELECT Id, Title, Description, CreationDateTime, (SELECT COUNT(*) FROM Messages WHERE ConversationId = Conversations.Id) AS TotalMessageCount, OwnerUserId " +
                                " FROM Conversations");

            foreach (Conversation conversation in conversations)
            {
                await LoadConversationMessagesAsync(conversation, connection);
                await LoadConversationMembersAsync(conversation, connection);
            }

            return new Chat(userId, conversations.ToList());

        }



        private async Task LoadConversationMembersAsync(Conversation conversation, IDbConnection connection)
        {
            IEnumerable<Chatter> conversationMembers = await connection
                    .QueryAsync<Chatter>(
                        "SELECT Id, UserName AS Name, JoinDateTime " +
                        " FROM Users " +
                        " WHERE Id IN (" +
                        "   SELECT UserId " +
                        "   FROM ConversationUsers " +
                        "   WHERE ConversationId = @ConversationId) ",
                        new { ConversationId = conversation.Id });

            conversation.ConversationMembers = conversationMembers.ToList();
        }

        private async Task LoadConversationMessagesAsync(Conversation conversation, IDbConnection connection)
        {
            IEnumerable<Message> conversationMessages = await connection
                .QueryAsync<Message>(
                    "SELECT Id, AuthorUserId AS AuthorChatterId, Text, MessageDateTime, ReplyMessageId " +
                    " FROM Messages " +
                    " WHERE ConversationId = @ConversationId",
                    new { ConversationId = conversation.Id });

            conversation.LoadedMessages = conversationMessages.ToList();
        }

    }
}
