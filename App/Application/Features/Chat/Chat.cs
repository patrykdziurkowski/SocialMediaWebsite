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
    public class Chat : AggreggateRoot
    {
        private readonly List<Conversation> _conversations;

        public Chat(
            ChatterId chatterId,
            List<Conversation> conversations)
        {
            CurrentChatterId = chatterId;
            _conversations = conversations;
        }

        public ChatterId CurrentChatterId { get; private set; }
        public IEnumerable<Conversation> Conversations => _conversations;

        public void CreateConversation(
            List<ChatterId> conversationMemberIds,
            string title,
            string? description = null)
        {
            Conversation newConversation = new(
                DateTimeOffset.Now,
                CurrentChatterId,
                conversationMemberIds,
                title,
                description);

            _conversations.Add(newConversation);
            RaiseDomainEvent(
                new ConversationCreatedEvent(
                    newConversation.Id,
                    title,
                    description,
                    CurrentChatterId,
                    conversationMemberIds));
        }


        public void LeaveConversation(Guid conversationId)
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

        public Result AddMemberToConversation(Guid conversationId, ChatterId chatterId)
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

        public Result KickMemberFromConversation(Guid conversationId, ChatterId chatterId)
        {
            Conversation conversationToKickMemberFrom = Conversations
                .Single(c => c.Id == conversationId);

            if (CurrentChatterId != conversationToKickMemberFrom.OwnerChatterId)
            {
                return Result.Fail("You cannot kick members from a conversation you do not own");
            }

            bool userExisted = conversationToKickMemberFrom.ConversationMemberIds.Remove(chatterId);
            if (!userExisted)
            {
                throw new InvalidOperationException("Chatter to be kicked was not found in the given conversation");
            }

            RaiseDomainEvent(
                new ConversationMemberKickedEvent(
                    conversationId,
                    chatterId));
            return Result.Ok();
        }


        public void PostMessage(
            Guid conversationId,
            string text,
            MessageId? replyMessageId = null)
        {
            Conversation conversationToPostIn = Conversations
                .Single(c => c.Id == conversationId);

            Message message = new(
                CurrentChatterId,
                text,
                DateTimeOffset.Now,
                replyMessageId);

            conversationToPostIn.LoadedMessages.Add(message);
            conversationToPostIn.TotalMessageCount++;

            RaiseDomainEvent(
                new MessagePostedEvent(
                    message.Id,
                    CurrentChatterId,
                    text,
                    conversationId,
                    replyMessageId));
        }

        public void DeleteMessage(
            Guid conversationId,
            MessageId messageId)
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
