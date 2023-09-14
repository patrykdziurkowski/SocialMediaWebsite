using Application.Features.Authentication.Interfaces;
using Application.Features.Authentication.Models;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Authentication.Commands
{
    public class RegisterCommand : IRegisterCommand
    {
        private readonly IUserRepository _userRepository;
        private readonly ISecretHasher _secretHasher;

        public RegisterCommand(
            IUserRepository userRepository,
            ISecretHasher secretHasher)
        {
            _userRepository = userRepository;
            _secretHasher = secretHasher;
        }

        public async Task<Result> Handle(UserRegisterModel inputUser)
        {
            Result<User> usernameAlreadyExistsResult = await _userRepository.GetUserByUserNameAsync(inputUser.UserName!);
            if (usernameAlreadyExistsResult.IsSuccess)
            {
                return Result.Fail("A user with such username already exists");
            }

            string passwordHash = _secretHasher.Hash(inputUser.Password!);
            User user = new(
                inputUser.UserName!,
                inputUser.Email!,
                passwordHash);

            Result result = await _userRepository.Register(user);
            return result;
        }

    }
}
