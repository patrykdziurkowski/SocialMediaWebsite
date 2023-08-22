using Application.Features.Authentication;
using Application.Features.Authentication.Interfaces;
using Application.Features.Authentication.Models;
using Application.Features.Authentication.Validators;
using Application.Features.Chat;
using FluentAssertions;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class AutheticationControllerTests
    {
        private readonly AuthenticationController _subject;

        private readonly RegisterValidator _registerValidator;
        private readonly LoginValidator _loginValidator;
        private readonly IUserRepository _userRepository;
        private readonly ISecretHasher _secretHasher;
        private readonly ISignInManager _signInManager;

        public AutheticationControllerTests()
        {
            _registerValidator = new();
            _loginValidator = new();
            _userRepository = Substitute.For<IUserRepository>();
            _secretHasher = Substitute.For<ISecretHasher>();
            _signInManager = Substitute.For<ISignInManager>();

            _subject = new(
                _userRepository,
                _secretHasher,
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
                
            _userRepository.Register(Arg.Any<User>()).Returns(Result.Ok());

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
            ObjectResult result = (ObjectResult) await _subject.Register(invalidUser);

            //Assert
            result.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Register_Post_ShouldReturn500_WhenDatabaseInsertionUnsuccessful()
        {
            //Arrange
            UserRegisterModel validUser = new()
            {
                UserName = "JohnSmith123",
                Email = "john@smith.com",
                Password = "P@ssword1"
            };     
                 
            _userRepository.Register(Arg.Any<User>()).Returns(Result.Fail(""));

            //Act
            StatusCodeResult result = (StatusCodeResult) await _subject.Register(validUser);

            //Assert
            result.StatusCode.Should().Be(500);
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
                

            _signInManager.SignIn(Arg.Any<HttpContext>(), validUser).Returns(Result.Ok());

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
            ObjectResult result = (ObjectResult) await _subject.Login(validUser);

            //Assert
            result.StatusCode.Should().Be(400);
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
                

            _signInManager.SignIn(Arg.Any<HttpContext>(), validUser).Returns(Result.Fail(""));

            //Act
            ObjectResult result = (ObjectResult) await _subject.Login(validUser);

            //Assert
            result.StatusCode.Should().Be(404);
        }

        

    }
}
