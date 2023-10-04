using Application.Features.Authentication.Commands;
using Application.Features.Authentication.Interfaces;
using Application.Features.Authentication.Models;
using Application.Features.Authentication.Validators;
using Application.Features.Conversations;
using FluentAssertions;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace Tests
{
    public class AuthenticationControllerTests
    {
        private readonly AuthenticationController _subject;

        private readonly RegisterValidator _registerValidator;
        private readonly LoginValidator _loginValidator;

        private readonly ISignInManager _signInManager;
        private readonly IRegisterCommand _registerCommand;

        public AuthenticationControllerTests()
        {
            _registerValidator = new();
            _loginValidator = new();
            _registerCommand = Substitute.For<IRegisterCommand>();
            _signInManager = Substitute.For<ISignInManager>();

            _subject = new(
                _registerCommand,
                _signInManager,
                _registerValidator,
                _loginValidator);
        }

        [Fact]
        public async Task Register_Post_ShouldReturn201_WhenUserIsValidAndSuccessfulyRegistered()
        {
            //Arrange
            UserRegisterModel validUser = new()
            {
                UserName = "JohnSmith123",
                Email = "john@smith.com",
                Password = "P@ssword1"
            };

            _registerCommand.Handle(validUser).Returns(Result.Ok());

            //Act
            StatusCodeResult result = (StatusCodeResult) await _subject.Register(validUser);

            //Assert
            result.StatusCode.Should().Be(201);
        }

        [Fact]
        public async Task Register_Post_ShouldReturn400_WhenUserIsInvalid()
        {
            //Arrange
            UserRegisterModel invalidUser = new()
            {
                UserName = "",
                Email = "",
                Password = ""
            };

            //Act
            IActionResult result = await _subject.Register(invalidUser);

            //Assert
            result.Should().BeOfType<ObjectResult>();
        }

        [Fact]
        public async Task Register_Post_ShouldReturn403_WhenRegisterCommandFailed()
        {
            //Arrange
            UserRegisterModel validUser = new()
            {
                UserName = "JohnSmith123",
                Email = "john@smith.com",
                Password = "P@ssword1"
            };

            _registerCommand.Handle(validUser).Returns(Result.Fail(""));

            //Act
            StatusCodeResult result = (StatusCodeResult) await _subject.Register(validUser);

            //Assert
            result.StatusCode.Should().Be(403);
        }



        [Fact]
        public async Task Login_Post_ShouldReturn200_WhenUserIsValidAndSuccessfulyLoggedIn()
        {
            //Arrange
            UserLoginModel validUser = new()
            {
                UserName = "JohnSmith123",
                Password = "P@ssword1"
            };


            _signInManager.SignIn(validUser).Returns(Result.Ok());

            //Act
            StatusCodeResult result = (StatusCodeResult) await _subject.Login(validUser);

            //Assert
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Login_Post_ShouldReturn400_WhenUserIsInvalid()
        {
            //Arrange
            UserLoginModel validUser = new()
            {
                UserName = "InvalidUser",
                Password = "InvalidPassword"
            };


            //Act
            IActionResult result = await _subject.Login(validUser);

            //Assert
            result.Should().BeOfType<ObjectResult>();
        }

        [Fact]
        public async Task Login_Post_ShouldReturn404_WhenUserNotLoggedInSuccessfuly()
        {
            //Arrange
            UserLoginModel validUser = new()
            {
                UserName = "NotFoundUser",
                Password = "P@ssword1!"
            };


            _signInManager.SignIn(validUser).Returns(Result.Fail(""));

            //Act
            ObjectResult result = (ObjectResult) await _subject.Login(validUser);

            //Assert
            result.StatusCode.Should().Be(404);
        }



    }
}
