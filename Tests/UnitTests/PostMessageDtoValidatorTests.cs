using Application.Features.Conversation.Dtos;
using Application.Features.Conversation.Validators;
using FluentAssertions;
using FluentValidation.Results;
using System.Text;
using Xunit;

namespace Tests.UnitTests
{
    public class PostMessageDtoValidatorTests
    {
        private PostMessageDtoValidator _subject;

        public PostMessageDtoValidatorTests()
        {
            _subject = new PostMessageDtoValidator();
        }

        [Fact]
        public void Validate_GivenNullText_Fails()
        {
            //Arrange
            PostMessageDto input = new()
            {
                Text = null,
                ReplyMessageId = null
            };

            //Act
            ValidationResult result = _subject.Validate(input);

            //Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_GivenStringEmptyText_Fails()
        {
            //Arrange
            PostMessageDto input = new()
            {
                Text = string.Empty,
                ReplyMessageId = null
            };

            //Act
            ValidationResult result = _subject.Validate(input);

            //Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_GivenTextOver500Characters_Fails()
        {
            //Arrange
            const char SomeCharacter = 'a';
            StringBuilder sb = new();
            for (int i = 0; i < 501; i++)
            {
                sb.Append(SomeCharacter);
            }

            PostMessageDto input = new()
            {
                Text = sb.ToString(),
                ReplyMessageId = null
            };

            //Act
            ValidationResult result = _subject.Validate(input);

            //Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_GivenValidString_Passes()
        {
            //Arrange
            PostMessageDto input = new()
            {
                Text = "Random 3xample message! 😀",
                ReplyMessageId = null
            };

            //Act
            ValidationResult result = _subject.Validate(input);

            //Assert
            result.IsValid.Should().BeTrue();
        }

    }
}
