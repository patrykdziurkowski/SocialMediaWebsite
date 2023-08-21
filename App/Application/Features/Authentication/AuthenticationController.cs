using Application.Features.Authentication;
using Application.Features.Authentication.Interfaces;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Chat
{
    public class AuthenticationController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<UserRegisterModel> _userRegisterModelValidator;

        public AuthenticationController(
            IUserRepository userRepository,
            IValidator<UserRegisterModel> userRegsiterModelValidator)
        {
            _userRepository = userRepository;
            _userRegisterModelValidator = userRegsiterModelValidator;
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterModel inputUser)
        {
            ValidationResult validationResult = _userRegisterModelValidator
                .Validate(inputUser);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            string passwordHash = SecretHasher.Hash(inputUser.Password);
            User user = new(
                inputUser.UserName,
                inputUser.Email,
                passwordHash);

            Result result = await _userRepository.Register(user);
            if (result.IsFailed)
            {
                return StatusCode(500);
            }

            return StatusCode(201);
        }

    }
}
