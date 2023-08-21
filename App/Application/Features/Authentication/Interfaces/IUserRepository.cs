using FluentResults;

namespace Application.Features.Authentication.Interfaces
{
    public interface IUserRepository
    {
        Task<Result> Register(User user);
    }
}