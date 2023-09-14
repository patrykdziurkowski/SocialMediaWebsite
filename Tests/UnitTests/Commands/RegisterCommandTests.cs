using Application.Features.Authentication;
using Application.Features.Authentication.Commands;
using Application.Features.Authentication.Interfaces;
using Application.Features.Authentication.Models;
using FluentAssertions;
using FluentResults;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.UnitTests.Commands
{
    public class RegisterCommandTests
    {
        private readonly RegisterCommand _subject;

        private readonly IUserRepository _userRepository;
        private readonly ISecretHasher _secretHasher;

        public RegisterCommandTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _secretHasher = Substitute.For<ISecretHasher>();

            _subject = new(
                _userRepository,
                _secretHasher);
        }

        [Fact]
        public async Task Handle_GivenExistingUser_ReturnsFail()
        {
            //Arrange
            UserRegisterModel user = new()
            {
                UserName = "TestUs3r",
                Password = "t3stPwd!",
                Email = "test@user.com"
            };
            _userRepository.GetUserByUserNameAsync(user.UserName).Returns(Result.Ok());

            //Act
            Result result = await _subject.Handle(user);

            //Assert
            result.IsFailed.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_GivenDatabaseRegisterFailed_ReturnsFail()
        {
            //Arrange
            UserRegisterModel user = new()
            {
                UserName = "TestUs3r",
                Password = "t3stPwd!",
                Email = "test@user.com"
            };

            _userRepository.GetUserByUserNameAsync(user.UserName).Returns(Result.Fail(""));
            _userRepository.Register(Arg.Any<User>()).Returns(Result.Fail(""));

            //Act
            Result result = await _subject.Handle(user);

            //Assert
            result.IsFailed.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_GivenRegisterSuccess_ReturnsOk()
        {
            //Arrange
            UserRegisterModel user = new()
            {
                UserName = "TestUs3r",
                Password = "t3stPwd!",
                Email = "test@user.com"
            };

            _userRepository.GetUserByUserNameAsync(user.UserName).Returns(Result.Fail(""));
            _userRepository.Register(Arg.Any<User>()).Returns(Result.Ok());

            //Act
            Result result = await _subject.Handle(user);

            //Assert
            result.IsSuccess.Should().BeTrue();
        }

    }
}
