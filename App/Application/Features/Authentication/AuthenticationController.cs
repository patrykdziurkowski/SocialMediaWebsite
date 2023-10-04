using Application.Features.Authentication;
using Application.Features.Authentication.Commands;
using Application.Features.Authentication.Interfaces;
using Application.Features.Authentication.Models;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.Chat
{
    public class AuthenticationController : Controller
    {
        private readonly IRegisterCommand _registerCommand;
        private readonly ISignInManager _signInManager;

        private readonly IValidator<UserRegisterModel> _registerValidator;
        private readonly IValidator<UserLoginModel> _loginValidator;

        public AuthenticationController(
            IRegisterCommand registerCommand,
            ISignInManager signInManager,
            IValidator<UserRegisterModel> registerValidator,
            IValidator<UserLoginModel> loginValidator)
        {
            _registerCommand = registerCommand;
            _signInManager = signInManager;

            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] UserRegisterModel inputUser)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            ValidationResult validationResult = _registerValidator
                .Validate(inputUser);
            if (!validationResult.IsValid)
            {
                validationResult.Errors.ForEach(f => ModelState.AddModelError(f.PropertyName, f.ErrorMessage));
                return ValidationProblem(ModelState);
            }

            Result<User> registerResult = await _registerCommand.Handle(inputUser);
            if (registerResult.IsFailed)
            {
                return StatusCode(403);
            }

            return Created("Login", new UserDto(registerResult.Value));
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] UserLoginModel inputUser)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            ValidationResult validationResult = _loginValidator.Validate(inputUser);
            if (!validationResult.IsValid)
            {
                validationResult.Errors.ForEach(f => ModelState.AddModelError(f.PropertyName, f.ErrorMessage));
                return ValidationProblem(ModelState);
            }

            Result signInResult = await _signInManager.SignIn(inputUser);
            if (signInResult.IsFailed)
            {
                return NotFound(signInResult.Errors.First().Message);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOut();
            return Ok();
        }
    }
}
