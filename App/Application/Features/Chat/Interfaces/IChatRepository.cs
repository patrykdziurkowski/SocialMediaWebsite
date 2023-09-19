namespace Application.Features.Chat.Interfaces
{
    public interface IChatRepository
    {
        Task<Chat> GetAsync(Guid userId);
        Task SaveAsync(Chat chat);
    }
}