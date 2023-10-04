using Application.Features.Chatter;
using Application.Features.Conversations.Events;
using Application.Features.Shared;
using FluentResults;

namespace Application.Features.Conversations
{
    public class Conversation : AggreggateRoot
    {
        private List<Message> _loadedMessages;
        private List<ChatterId> _conversationMemberIds;

        private Conversation()
        {
            Id = default!;
            Title = default!;
            Description = default!;
            CreationDateTime = default!;
            TotalMessageCount = default!;
            OwnerChatterId = default!;
            _loadedMessages = new List<Message>();
            _conversationMemberIds = new List<ChatterId>();
        }

        private Conversation(
            DateTimeOffset creationDateTime,
            ChatterId ownerChatterId,
            List<ChatterId> conversationMemberIds,
            string title,
            string? description = null)
        {
            Id = new ConversationId();
            CreationDateTime = creationDateTime;
            TotalMessageCount = 0;
            OwnerChatterId = ownerChatterId;
            _loadedMessages = new List<Message>();
            _conversationMemberIds = conversationMemberIds;
            Title = title;
            Description = description;
        }

        public ConversationId Id { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public DateTimeOffset CreationDateTime { get; private set; }
        public int TotalMessageCount { get; private set; }
        public ChatterId OwnerChatterId { get; private set; }
        public IEnumerable<Message> LoadedMessages
        {
            get => _loadedMessages;
            set
            {
                _loadedMessages = value.ToList();
            }
        }
        public IEnumerable<ChatterId> ConversationMemberIds
        {
            get => _conversationMemberIds;
            set
            {
                _conversationMemberIds = value.ToList();
            }
        }


        public static Conversation Start(
            ChatterId currentChatterId,
            List<ChatterId> conversationMemberIds,
            string title,
            string? description = null)
        {
            Conversation newConversation = new(
                DateTimeOffset.Now,
                currentChatterId,
                conversationMemberIds,
                title,
                description);

            newConversation.RaiseDomainEvent(
                new ConversationStartedEvent(
                    newConversation.Id,
                    title,
                    description,
                    currentChatterId,
                    conversationMemberIds));

            return newConversation;
        }


        public void Leave(ChatterId currentChatterId)
        {
            _conversationMemberIds.Remove(currentChatterId);
            RaiseDomainEvent(
                new ConversationLeftEvent(
                    Id,
                    currentChatterId));

            if (!ConversationMemberIds.Any())
            {
                RaiseDomainEvent(new ConversationDeletedEvent(Id));
            }
        }

        public Result AddMember(ChatterId currentChatterId, ChatterId chatterId)
        {
            if (currentChatterId != OwnerChatterId)
            {
                return Result.Fail("You cannot add members to a conversation you do not own");
            }
            if (ConversationMemberIds.Contains(chatterId))
            {
                return Result.Fail("That chatter is already in this conversation");
            }

            _conversationMemberIds.Add(chatterId);
            RaiseDomainEvent(
                new ConversationMemberAddedEvent(
                    Id,
                    chatterId));
            return Result.Ok();
        }

        public Result KickMember(ChatterId currentChatterId, ChatterId chatterId)
        {
            if (currentChatterId != OwnerChatterId)
            {
                return Result.Fail("You cannot kick members from a conversation you do not own");
            }

            bool userExisted = _conversationMemberIds.Remove(chatterId);
            if (!userExisted)
            {
                throw new InvalidOperationException("Chatter to be kicked was not found in the given conversation");
            }

            RaiseDomainEvent(
                new ConversationMemberKickedEvent(
                    Id,
                    chatterId));
            return Result.Ok();
        }


        public void PostMessage(
            ChatterId currentChatterId,
            string text,
            MessageId? replyMessageId = null)
        {
            Message message = new(
                currentChatterId,
                text,
                DateTimeOffset.Now,
                replyMessageId);

            _loadedMessages.Add(message);
            TotalMessageCount++;

            RaiseDomainEvent(
                new MessagePostedEvent(
                    message.Id,
                    currentChatterId,
                    text,
                    Id,
                    replyMessageId));
        }

        public void DeleteMessage(
            ChatterId currentChatterId,
            MessageId messageId)
        {
            Message messageToDelete = LoadedMessages
                .Single(m => m.Id == messageId);

            if (messageToDelete.AuthorChatterId != currentChatterId)
            {
                throw new InvalidOperationException("Attempted to delete a message that doesn't belong to this user");
            }

            _loadedMessages.Remove(messageToDelete);
            TotalMessageCount--;

            RaiseDomainEvent(
                new MessageDeletedEvent(messageId));
        }

    }
}
