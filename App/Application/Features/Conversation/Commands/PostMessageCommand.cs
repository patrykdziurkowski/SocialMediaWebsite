using Application.Features.Chatter;
using Application.Features.Conversations.Dtos;
using Application.Features.Conversations.Interfaces;

namespace Application.Features.Conversations.Commands
{
    public class PostMessageCommand : IPostMessageCommand
    {
        private readonly IConversationRepository _conversationRepository;

        public PostMessageCommand(IConversationRepository conversationRepository)
        {
            _conversationRepository = conversationRepository;
        }

        public async Task<Message> Handle(
            ChatterId currentChatterId,
            ConversationId conversationId,
            PostMessageModel input)
        {
            MessageId? replyMessageId = null;
            if (input.ReplyMessageId is not null)
            {
                replyMessageId = new MessageId((Guid) input.ReplyMessageId!);
            }

            Conversation conversation = await _conversationRepository.GetByIdAsync(
                currentChatterId,
                conversationId);
            conversation.PostMessage(
                currentChatterId,
                input.Text!,
                replyMessageId);
            await _conversationRepository.SaveAsync(conversation);

            return conversation.LoadedMessages.Last();
        }

    }
}
