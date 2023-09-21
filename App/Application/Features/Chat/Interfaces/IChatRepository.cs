using Application.Features.Chatter;

namespace Application.Features.Chat.Interfaces
{
    public interface IChatRepository
    {
        Task<Chat> GetAsync(ChatterId userId);
        Task SaveAsync(Chat chat);
    }
}