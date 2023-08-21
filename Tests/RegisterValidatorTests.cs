using Application.Features.Authentication;
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
    public class RegisterValidatorTests
    {
        [Fact]
        public void Validate_Passes_WhenUserIsValid()
        {
            //Arrange
            UserRegisterModel user = new(
                "JohnSmith123",
                "jsmith@email.com",
                "P@ssword1!");
            RegisterValidator subject = new();

            //Act
            ValidationResult result = subject.Validate(user);

            //Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_Fails_WhenUserNameIsShorterThan6Chars()
        {
            //Arrange
            UserRegisterModel user = new(
                "JohnS",
                "jsmith@email.com",
                "P@ssword1!");
            RegisterValidator subject = new();

            //Act
            ValidationResult result = subject.Validate(user);

            //Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_Fails_WhenUserNameIsLongerThan20Chars()
        {
            //Arrange
            UserRegisterModel user = new(
                "JonathanSmith12345678",
                "jsmith@email.com",
                "P@ssword1!");
            RegisterValidator subject = new();

            //Act
            ValidationResult result = subject.Validate(user);

            //Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_Fails_WhenUserNameIsNull()
        {
            //Arrange
            UserRegisterModel user = new(
                null!,
                "jsmith@email.com",
                "P@ssword1!");
            RegisterValidator subject = new();

            //Act
            ValidationResult result = subject.Validate(user);

            //Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_Fails_WhenUserNameIsNotAlphaNumeric()
        {
            //Arrange
            UserRegisterModel user = new(
                "JohnSm!th",
                "jsmith@email.com",
                "P@ssword1!");
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
            UserRegisterModel user = new(
                "JohnSmith123",
                email,
                "P@ssword1!");
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
            UserRegisterModel user = new(
                "JohnSmith123",
                null!,
                "P@ssword1!");
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
            UserRegisterModel user = new(
                "JohnSmith123",
                Generate320CharacterString(),
                "P@ssword1!");
            RegisterValidator subject = new();

            //Act
            ValidationResult result = subject.Validate(user);

            //Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_Fails_WhenPasswordIsShorterThan8()
        {
            //Arrange
            UserRegisterModel user = new(
                "JohnSmith123",
                "john@smith.com",
                "P@sswor");
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
            UserRegisterModel user = new(
                "JohnSmith123",
                "john@smith.com",
                null!);
            RegisterValidator subject = new();

            //Act
            ValidationResult result = subject.Validate(user);

            //Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_Fails_WhenPasswordHasNoUppercaseLetter()
        {
            //Arrange
            UserRegisterModel user = new(
                "JohnSmith123",
                "john@smith.com",
                "p@ssword1!");
            RegisterValidator subject = new();

            //Act
            ValidationResult result = subject.Validate(user);

            //Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_Fails_WhenPasswordHasNoLowercaseLetter()
        {
            //Arrange
            UserRegisterModel user = new(
                "JohnSmith123",
                "john@smith.com",
                "P@SSWORD1!");
            RegisterValidator subject = new();

            //Act
            ValidationResult result = subject.Validate(user);

            //Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_Fails_WhenPasswordHasNoDigit()
        {
            //Arrange
            UserRegisterModel user = new(
                "JohnSmith123",
                "john@smith.com",
                "P@ssword!");
            RegisterValidator subject = new();

            //Act
            ValidationResult result = subject.Validate(user);

            //Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_Fails_WhenPasswordHasNoSpecialSymbol()
        {
            //Arrange
            UserRegisterModel user = new(
                "JohnSmith123",
                "john@smith.com",
                "Password1");
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
