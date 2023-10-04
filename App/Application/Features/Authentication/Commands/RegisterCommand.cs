using Application.Features.Authentication.Interfaces;
using Application.Features.Authentication.Models;
using FluentResults;

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

        public async Task<Result<User>> Handle(UserRegisterModel inputUser)
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

            Result<User> result = await _userRepository.Register(user);
            return result;
        }

    }
}
