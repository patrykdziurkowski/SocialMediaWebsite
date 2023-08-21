using Application;
using Application.Features.Authentication;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class FluentValidationCustomRulesTests
    {
        [Theory]
        [InlineData("1")]
        [InlineData("5")]
        [InlineData("0@")]
        [InlineData("4a")]
        [InlineData("longString4Digits7")]
        public void MustContainADigit_GivenStringWithDigit_Passes(string stringWithADigit)
        {
            //Arrange
            InlineValidator<string> validator = new();
            validator
                .RuleFor(x => x.ToString())
                .MustContainADigit();

            //Act
            ValidationResult result = validator.Validate(stringWithADigit);

            //Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("a")]
        [InlineData("jiasdfsd@")]
        [InlineData("@@@sdfasxcvpl")]
        public void MustContainADigit_GivenStringWithNoDigit_Fails(string stringWithADigit)
        {
            //Arrange
            InlineValidator<string> validator = new();
            validator
                .RuleFor(x => x.ToString())
                .MustContainADigit();

            //Act
            ValidationResult result = validator.Validate(stringWithADigit);

            //Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void MustContainADigit_GivenEmptyString_Fails()
        {
            //Arrange
            InlineValidator<string> validator = new();
            validator
                .RuleFor(x => x.ToString())
                .MustContainADigit();

            string stringWithADigit = "";

            //Act
            ValidationResult result = validator.Validate(stringWithADigit);

            //Assert
            result.IsValid.Should().BeFalse();
        }



        [Theory]
        [InlineData("A")]
        [InlineData("1F")]
        [InlineData("AbCD")]
        [InlineData("0@L")]
        [InlineData("4LLa")]
        public void MustContainAnUppercase_GivenStringWithUppercase_Passes(string stringWithUppercase)
        {
            //Arrange
            InlineValidator<string> validator = new();
            validator
                .RuleFor(x => x.ToString())
                .MustContainAnUppercase();

            //Act
            ValidationResult result = validator.Validate(stringWithUppercase);

            //Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("a")]
        [InlineData("1f")]
        [InlineData("abcd")]
        [InlineData("0@l")]
        [InlineData("4lla")]
        public void MustContainAnUppercase_GivenStringWithNoUppercase_Fails(string stringWithNoUppercase)
        {
            //Arrange
            InlineValidator<string> validator = new();
            validator
                .RuleFor(x => x.ToString())
                .MustContainAnUppercase();

            //Act
            ValidationResult result = validator.Validate(stringWithNoUppercase);

            //Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void MustContainAnUppercase_GivenStringEmpty_Fails()
        {
            //Arrange
            InlineValidator<string> validator = new();
            validator
                .RuleFor(x => x.ToString())
                .MustContainAnUppercase();

            string emptyString = "";

            //Act
            ValidationResult result = validator.Validate(emptyString);

            //Assert
            result.IsValid.Should().BeFalse();
        }

        [Theory]
        [InlineData("a")]
        [InlineData("1f")]
        [InlineData("AbCD")]
        [InlineData("0@l")]
        [InlineData("4lLa")]
        public void MustContainALowercase_GivenStringWithLowercase_Passes(string stringWithLowercase)
        {
            //Arrange
            InlineValidator<string> validator = new();
            validator
                .RuleFor(x => x.ToString())
                .MustContainALowercase();

            //Act
            ValidationResult result = validator.Validate(stringWithLowercase);

            //Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("A")]
        [InlineData("1F")]
        [InlineData("ABCD")]
        [InlineData("0@L")]
        [InlineData("4LLA")]
        public void MustContainALowercase_GivenStringWithNoLowercase_Fails(string stringWithNoLowercase)
        {
            //Arrange
            InlineValidator<string> validator = new();
            validator
                .RuleFor(x => x.ToString())
                .MustContainALowercase();

            //Act
            ValidationResult result = validator.Validate(stringWithNoLowercase);

            //Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void MustContainALowercase_GivenStringEmpty_Fails()
        {
            //Arrange
            InlineValidator<string> validator = new();
            validator
                .RuleFor(x => x.ToString())
                .MustContainALowercase();

            string emptyString = "";

            //Act
            ValidationResult result = validator.Validate(emptyString);

            //Assert
            result.IsValid.Should().BeFalse();
        }


        [Theory]
        [InlineData("A")]
        [InlineData("A22")]
        [InlineData("AlphanumericOnlyString1")]
        public void ContainsOnlyAlphanumeric_GivenAlphanumericOnlyString_Passes(string alphanumericString)
        {
            //Arrange
            InlineValidator<string> validator = new();
            validator
                .RuleFor(x => x.ToString())
                .MustContainOnlyAlphanumeric();

            //Act
            ValidationResult result = validator.Validate(alphanumericString);

            //Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("$")]
        [InlineData("@22")]
        [InlineData("NonAlphanumericOnlyString1!")]
        public void ContainsOnlyAlphanumeric_GivenNonAlphanumericString_Fails(string nonAlphanumericString)
        {
            //Arrange
            InlineValidator<string> validator = new();
            validator
                .RuleFor(x => x.ToString())
                .MustContainOnlyAlphanumeric();

            //Act
            ValidationResult result = validator.Validate(nonAlphanumericString);

            //Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void ContainsOnlyAlphanumeric_GivenEmptyString_Passes()
        {
            //Arrange
            InlineValidator<string> validator = new();
            validator
                .RuleFor(x => x.ToString())
                .MustContainOnlyAlphanumeric();

            string emptyString = "";

            //Act
            ValidationResult result = validator.Validate(emptyString);

            //Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("@")]
        [InlineData("!22")]
        [InlineData("Special$Character]String")]
        [InlineData("`~!@#$%^&*()-_=+[]{}\\|;:\'\",<.>/?")]
        public void MustContainASpecialCharacter_GivenStringWithSpecialCharacter_Passes(string specialCharacterString)
        {
            //Arrange
            InlineValidator<string> validator = new();
            validator
                .RuleFor(x => x.ToString())
                .MustContainASpecialCharacter();

            //Act
            ValidationResult result = validator.Validate(specialCharacterString);

            //Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("A")]
        [InlineData("A22")]
        [InlineData("NoSpecialCharacters12123231")]
        public void MustContainASpecialCharacter_GivenStringWithNoSpecialCharacters_Fails(string noSpecialCharactersString)
        {
            //Arrange
            InlineValidator<string> validator = new();
            validator
                .RuleFor(x => x.ToString())
                .MustContainASpecialCharacter();

            //Act
            ValidationResult result = validator.Validate(noSpecialCharactersString);

            //Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void MustContainASpecialCharacter_GivenEmptyString_Fails()
        {
            //Arrange
            InlineValidator<string> validator = new();
            validator
                .RuleFor(x => x.ToString())
                .MustContainASpecialCharacter();

            string emptyString = "";

            //Act
            ValidationResult result = validator.Validate(emptyString);

            //Assert
            result.IsValid.Should().BeFalse();
        }

    }
}
