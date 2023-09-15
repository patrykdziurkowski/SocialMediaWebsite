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
            CurrentChatterId = chatterId;
            _conversations = conversations;
            _domainEvents = new List<DomainEvent>();
        }

        public int CurrentChatterId { get; private set; }
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
            List<int> conversationMemberIds,
            string title,
            string? description = null)
        {
            Conversation newConversation = new(
                CurrentChatterId,
                conversationMemberIds,
                title,
                description);

            _conversations.Add(newConversation);
            RaiseDomainEvent(
                new ConversationCreatedEvent(
                    title,
                    description,
                    CurrentChatterId,
                    conversationMemberIds));
        }


        public void LeaveConversation(int conversationId)
        {
            Conversation conversationToLeave = Conversations
                .Single(c => c.Id == conversationId);

            conversationToLeave.ConversationMemberIds.Remove(CurrentChatterId);
            _conversations.Remove(conversationToLeave);
            RaiseDomainEvent(
                new ConversationLeftEvent(
                    conversationId,
                    CurrentChatterId));

            if (conversationToLeave.ConversationMemberIds.Count == 0)
            {
                RaiseDomainEvent(new ConversationDeletedEvent(conversationId));
            }
        }

        public Result AddMemberToConversation(int conversationId, int chatterId)
        {
            Conversation conversationToAddMemberTo = Conversations
                .Single(c => c.Id == conversationId);

            if (conversationToAddMemberTo.OwnerChatterId != CurrentChatterId)
            {
                return Result.Fail("You cannot add members to a conversation you do not own");
            }
            if (conversationToAddMemberTo.ConversationMemberIds.Contains(chatterId))
            {
                return Result.Fail("That chatter is already in this conversation");
            }

            conversationToAddMemberTo.ConversationMemberIds.Add(chatterId);
            RaiseDomainEvent(
                new ConversationMemberAddedEvent(
                    conversationId,
                    chatterId));
            return Result.Ok();
        }

        public void KickMemberFromConversation(int conversationId, int chatterId)
        {
            Conversation conversationToKickMemberFrom = Conversations
                .Single(c => c.Id == conversationId);

            bool userExisted = conversationToKickMemberFrom.ConversationMemberIds.Remove(chatterId);
            if (!userExisted)
            {
                throw new InvalidOperationException("Chatter to be kicked was not found in the given conversation");
            }

            RaiseDomainEvent(
                new ConversationMemberKickedEvent(
                    conversationId,
                    chatterId));
        }


        public void PostMessage(
            int conversationId,
            string text,
            int? replyMessageId = null)
        {
            Conversation conversationToPostIn = Conversations
                .Single(c => c.Id == conversationId);

            Message message = new(
                CurrentChatterId,
                text,
                replyMessageId);

            conversationToPostIn.LoadedMessages.Add(message);
            conversationToPostIn.TotalMessageCount++;

            RaiseDomainEvent(
                new MessagePostedEvent(
                    CurrentChatterId,
                    text,
                    conversationId,
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

            if (messageToDelete.AuthorChatterId != CurrentChatterId)
            {
                throw new InvalidOperationException("Attempted to delete a message that doesn't belong to this user");
            }

            conversationToDeleteFrom.LoadedMessages.Remove(messageToDelete);
            conversationToDeleteFrom.TotalMessageCount--;

            RaiseDomainEvent(
                new MessageDeletedEvent(messageId));
        }

    }
}
