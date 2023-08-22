using Application.Features.Authentication.Models;
using FluentResults;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Authentication.Interfaces
{
    public interface ISignInManager
    {
        Task<Result> SignIn(HttpContext httpContext, UserLoginModel inputUser);
        Task SignOut(HttpContext httpContext);
    }
}