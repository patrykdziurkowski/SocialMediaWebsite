using Application.Features.Authentication.Models;
using FluentResults;

namespace Application.Features.Authentication.Commands
{
    public interface IRegisterCommand
    {
        Task<Result> Handle(UserRegisterModel inputUser);
    }
}