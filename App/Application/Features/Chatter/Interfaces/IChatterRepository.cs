namespace Application.Features.Chatter.Interfaces
{
    public interface IChatterRepository
    {
        Task<Chatter> GetByIdAsync(ChatterId chatterId);
    }
}