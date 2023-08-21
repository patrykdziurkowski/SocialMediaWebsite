﻿using Application.Features.Authentication;
using Application.Features.Authentication.Interfaces;
using Application.Features.Authentication.Models;
using Application.Features.Authentication.Validators;
using Application.Features.Chat;
using FluentAssertions;
using FluentResults;
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

        public AutheticationControllerTests()
        {
            _registerValidator = new();
            _loginValidator = new();
            _userRepository = Substitute.For<IUserRepository>();
            _secretHasher = Substitute.For<ISecretHasher>();
            
            _subject = new(
                _userRepository,
                _secretHasher,
                _registerValidator,
                _loginValidator);
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



        [Fact]
        public async Task Login_Post_ShouldReturn200_WhenUserIsValid()
        {
            //Arrange
            UserLoginModel validUser = new(
                "JohnSmith123",
                "P@ssword1");
            User user = new(
                "JohnSmith123",
                "email@email.com",
                "someHashedPassword");

            _userRepository.GetUserByUserNameAsync("JohnSmith123").Returns(Result.Ok(user));
            _secretHasher.Verify(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

            //Act
            StatusCodeResult result = (StatusCodeResult) await _subject.Login(validUser);

            //Assert
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Login_Post_ShouldReturn400_WhenUserIsInvalid()
        {
            //Arrange
            UserLoginModel validUser = new(
                "InvalidUser",
                "Password");

            //Act
            ObjectResult result = (ObjectResult) await _subject.Login(validUser);

            //Assert
            result.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Login_Post_ShouldReturn404_WhenUserNotFoundInDatabase()
        {
            //Arrange
            UserLoginModel validUser = new(
                "NotFoundUser",
                "P@ssword1!");

            _userRepository.GetUserByUserNameAsync("NotFoundUser").Returns(Result.Fail(""));

            //Act
            ObjectResult result = (ObjectResult) await _subject.Login(validUser);

            //Assert
            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task Login_Post_ShouldReturn404_WhenUserFoundInDatabaseButPasswordWrong()
        {
            //Arrange
            UserLoginModel validUser = new(
                "FoundUser",
                "P@ssword1!");
            User user = new(
                "FoundUser",
                "email@email.com",
                "someHashedPassword");

            _userRepository.GetUserByUserNameAsync("FoundUser").Returns(Result.Ok(user));
            _secretHasher.Verify(Arg.Any<string>(), Arg.Any<string>()).Returns(false);

            //Act
            ObjectResult result = (ObjectResult) await _subject.Login(validUser);

            //Assert
            result.StatusCode.Should().Be(404);
        }

    }
}
