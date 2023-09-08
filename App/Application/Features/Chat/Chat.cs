using Application.Features.Chat.Events;
using Application.Features.Shared;
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
        private readonly List<DomainEvent> _domainEvents;

        public Chat(
            int chatterId,
            List<Conversation> conversations)
        {
            ChatterId = chatterId;
            _conversations = conversations;
            _domainEvents = new List<DomainEvent>();
        }

        public int ChatterId { get; private set; }
        public IEnumerable<Conversation> Conversations => _conversations;
        public IEnumerable<DomainEvent> DomainEvents => _domainEvents;


        public void RaiseDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

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
            RaiseDomainEvent(
                new ConversationCreatedEvent(
                    title,
                    description,
                    ChatterId,
                    conversationMembers));
        }


        public void LeaveConversation(int conversationId)
        {
            Conversation conversationToLeave = Conversations
                .Single(c => c.Id == conversationId);

            conversationToLeave.ConversationMembers.RemoveAll(m => m.Id == ChatterId);
            _conversations.Remove(conversationToLeave);
            RaiseDomainEvent(
                new ConversationLeftEvent(
                    (int) conversationToLeave.Id!,
                    ChatterId));
        }


        public void PostMessage(
            int conversationId,
            string text,
            int? replyMessageId = null)
        {
            Conversation conversationToPostIn = Conversations
                .Single(c => c.Id == conversationId);

            Message message = new(
                ChatterId,
                text,
                replyMessageId);

            conversationToPostIn.LoadedMessages.Add(message);
            conversationToPostIn.TotalMessageCount++;

            RaiseDomainEvent(
                new MessagePostedEvent(
                    ChatterId,
                    text,
                    (int)conversationToPostIn.Id!,
                    replyMessageId));
        }

        public void DeleteMessage(
            int conversationId,
            int messageId)
        {
            Conversation conversationToDeleteFrom = Conversations
                .Single(c => c.Id == conversationId);

            Message messageToDelete = conversationToDeleteFrom.LoadedMessages
                .Single(m => m.Id == messageId);

            conversationToDeleteFrom.LoadedMessages.Remove(messageToDelete);
        }

    }
}
