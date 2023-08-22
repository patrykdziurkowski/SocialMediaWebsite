using Application.Features.Authentication.Models;
using Application.Features.Authentication.Validators;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    //Most password and username validation is found in FluentValidationCustomRulesTests
    public class RegisterValidatorTests
    {
        [Fact]
        public void Validate_Passes_WhenUserIsValid()
        {
            //Arrange
            UserRegisterModel user = new()
            {
                UserName = "JohnSmith123",
                Email = "jsmith@email.com",
                Password = "P@ssword1!"
            };
                
            RegisterValidator subject = new();

            //Act
            ValidationResult result = subject.Validate(user);

            //Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_Fails_WhenUserNameIsNull()
        {
            //Arrange
            UserRegisterModel user = new()
            {
                UserName = null,
                Email = "jsmith@email.com",
                Password = "P@ssword1!"
            };
                
            RegisterValidator subject = new();

            //Act
            ValidationResult result = subject.Validate(user);

            //Assert
            result.IsValid.Should().BeFalse();
        }

        [Theory]
        [InlineData("jsmith.com")]
        [InlineData("j!smith.com")]
        [InlineData("jsmithcom")]
        public void Validate_Fails_WhenEmailNotInEmailFormat(string email)
        {
            //Arrange
            UserRegisterModel user = new()
            {
                UserName = "JohnSmith123",
                Email = email,
                Password = "P@ssword1!"
            };
                
            RegisterValidator subject = new();

            //Act
            ValidationResult result = subject.Validate(user);

            //Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_Fails_WhenEmailIsNull()
        {
            //Arrange
            UserRegisterModel user = new()
            {
                UserName = "JohnSmith123",
                Email = null,
                Password = "P@ssword1!"
            };

            RegisterValidator subject = new();

            //Act
            ValidationResult result = subject.Validate(user);

            //Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_Fails_WhenEmailIsLongerThan319Characters()
        {
            //Arrange
            UserRegisterModel user = new()
            {
                UserName = "JohnSmith123",
                Email = Generate320CharacterString(),
                Password = "P@ssword1!"
            };

            RegisterValidator subject = new();

            //Act
            ValidationResult result = subject.Validate(user);

            //Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_Fails_WhenPasswordIsNull()
        {
            //Arrange
            UserRegisterModel user = new()
            {
                UserName = "JohnSmith123",
                Email = "john@smith.com",
                Password = null
            };

            RegisterValidator subject = new();

            //Act
            ValidationResult result = subject.Validate(user);

            //Assert
            result.IsValid.Should().BeFalse();
        }

        private static string Generate320CharacterString()
        {
            StringBuilder stringBuilder = new();

            for (int i = 0; i < 319; i++)
            {
                stringBuilder.Append('0');
            }

            return stringBuilder.ToString();
        }
    }
}
