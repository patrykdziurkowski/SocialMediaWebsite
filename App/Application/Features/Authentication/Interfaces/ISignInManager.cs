using Application.Features.Authentication.Models;
using FluentResults;

namespace Application.Features.Authentication.Interfaces
{
    public interface ISignInManager
    {
        Task<Result> SignIn(UserLoginModel inputUser);
        Task SignOut();
    }
}