using Application.Features.Chatter;

namespace Application.Features.Conversations.Interfaces
{
    public interface IConversationRepository
    {
        Task<IEnumerable<Conversation>> GetAllAsync(ChatterId currentChatterId);
        Task<Conversation> GetByIdAsync(ChatterId currentChatterId, ConversationId conversationId);
        Task SaveAsync(Conversation conversation);
    }
}