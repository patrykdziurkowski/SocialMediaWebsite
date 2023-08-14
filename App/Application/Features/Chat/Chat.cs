using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Chat
{
    public class Chat
    {
        private readonly List<Conversation> _conversations;

        public Chat(
            int chatterId,
            List<Conversation> conversations)
        {
            ChatterId = chatterId;
            _conversations = conversations;
        }

        public int ChatterId { get; private set; }
        public IEnumerable<Conversation> Conversations => _conversations;



        public void CreateConversation(
            List<Chatter> conversationMembers,
            string title,
            string? description = null)
        {
            Conversation newConversation = new(
                ChatterId,
                conversationMembers,
                title,
                description);

            _conversations.Add(newConversation);
        }



        public Result LeaveConversation(int conversationId)
        {
            Conversation? conversationToLeave = Conversations
                .SingleOrDefault(c => c.Id == conversationId);
            if (conversationToLeave is null)
            {
                return Result.Fail("No conversation with such id was found.");
            }

            conversationToLeave.ConversationMembers.RemoveAll(m => m.Id == ChatterId);
            _conversations.Remove(conversationToLeave);

            return Result.Ok();
        }


        public Result PostMessage(
            int conversationId,
            string text,
            int? replyMessageId = null)
        {
            Conversation? conversationToPostIn = Conversations
                .SingleOrDefault(c => c.Id == conversationId);
            if (conversationToPostIn is null)
            {
                return Result.Fail("No conversation with such id was found.");
            }

            Message message = new(
                ChatterId,
                text,
                replyMessageId);

            conversationToPostIn.LoadedMessages.Add(message);
            conversationToPostIn.TotalMessageCount++;

            return Result.Ok();
        }
    }
}
