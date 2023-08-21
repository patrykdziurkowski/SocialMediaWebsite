using Application.Features.Authentication;
using Application.Features.Authentication.Interfaces;
using Application.Features.Authentication.Validators;
using Application.Features.Chat;
using FluentAssertions;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class AutheticationControllerTests
    {
        private readonly AuthenticationController _subject;

        private readonly RegisterValidator _registerValidator;
        private readonly IUserRepository _userRepository;

        public AutheticationControllerTests()
        {
            _registerValidator = new();
            _userRepository = Substitute.For<IUserRepository>();

            _subject = new(
                _userRepository,
                _registerValidator);
        }

        [Fact]
        public async Task Register_Post_ShouldReturn201_WhenUserIsValidAndSuccessfulyRegistered()
        {
            //Arrange
            UserRegisterModel validUser = new(
                "JohnSmith123",
                "john@smith.com",
                "P@ssword1");
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
            UserRegisterModel invalidUser = new(
                "",
                "",
                "");

            //Act
            ObjectResult result = (ObjectResult) await _subject.Register(invalidUser);

            //Assert
            result.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Register_Post_ShouldReturn500_WhenDatabaseInsertionUnsuccessful()
        {
            //Arrange
            UserRegisterModel validUser = new(
                 "JohnSmith123",
                 "john@smith.com",
                 "P@ssword1");
            _userRepository.Register(Arg.Any<User>()).Returns(Result.Fail(""));

            //Act
            StatusCodeResult result = (StatusCodeResult) await _subject.Register(validUser);

            //Assert
            result.StatusCode.Should().Be(500);
        }



    }
}
