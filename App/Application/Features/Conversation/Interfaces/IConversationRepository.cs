using Application.Features.Chatter;

namespace Application.Features.Chat.Interfaces
{
    public interface IConversationRepository
    {
        Task<IEnumerable<Conversation>> GetAllAsync(ChatterId currentChatterId);
        Task<Conversation> GetByIdAsync(ChatterId currentChatterId, ConversationId conversationId);
        Task SaveAsync(Conversation conversation);
    }
}