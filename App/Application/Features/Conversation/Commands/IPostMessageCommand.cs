using Application.Features.Chatter;
using Application.Features.Conversations.Dtos;

namespace Application.Features.Conversations.Commands
{
    public interface IPostMessageCommand
    {
        Task<Message> Handle(
            ChatterId currentChatterId,
            ConversationId conversationId,
            PostMessageModel input);
    }
}