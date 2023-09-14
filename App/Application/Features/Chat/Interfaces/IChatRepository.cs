namespace Application.Features.Chat.Interfaces
{
    public interface IChatRepository
    {
        Task<Chat> GetAsync(int userId);
        Task SaveAsync(Chat chat);
    }
}