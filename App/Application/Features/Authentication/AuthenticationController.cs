using Application.Features.Authentication;
using Application.Features.Authentication.Interfaces;
using Application.Features.Authentication.Models;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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
        private readonly ISecretHasher _secretHasher;
        private readonly ISignInManager _signInManager;

        private readonly IValidator<UserRegisterModel> _registerValidator;
        private readonly IValidator<UserLoginModel> _loginValidator;

        public AuthenticationController(
            IUserRepository userRepository,
            ISecretHasher secretHasher,
            ISignInManager signInManager,
            IValidator<UserRegisterModel> registerValidator,
            IValidator<UserLoginModel> loginValidator)
        {
            _userRepository = userRepository;
            _secretHasher = secretHasher;
            _signInManager = signInManager;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterModel inputUser)
        {
            ValidationResult validationResult = _registerValidator
                .Validate(inputUser);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            string passwordHash = _secretHasher.Hash(inputUser.Password!);
            User user = new(
                inputUser.UserName!,
                inputUser.Email!,
                passwordHash);

            Result result = await _userRepository.Register(user);
            if (result.IsFailed)
            {
                return StatusCode(500);
            }

            return StatusCode(201);
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel inputUser)
        {
            ValidationResult validationResult = _loginValidator.Validate(inputUser);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            Result signInResult = await _signInManager.SignIn(inputUser);
            if (signInResult.IsFailed)
            {
                return NotFound(signInResult.Errors.First().Message);
            }

            return Ok();
        }
    }
}
