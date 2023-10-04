using FluentResults;

namespace Application.Features.Authentication.Interfaces
{
    public interface IUserRepository
    {
        Task<Result<User>> GetUserByUserNameAsync(string userName);
        Task<Result<User>> Register(User user);
    }
}