using Application.Features.Chat;
using Application.Features.Chat.Dtos;
using Application.Features.Chat.Interfaces;
using Application.Features.Chat.Validators;
using Application.Features.Chatter;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Security.Claims;
using Xunit;

namespace Tests.UnitTests
{
    public class ConversationControllerTests
    {
        private readonly ConversationController _subject;

        private readonly IConversationRepository _conversationRepository;
        private readonly ConversationCreationDtoValidator _conversationCreationValidator;

        private readonly ChatterId _currentChatterId;
        private readonly ChatterId _chatterInConversationId;
        private readonly ChatterId _chatterNotInConversationId;

        public ConversationControllerTests()
        {
            _currentChatterId = new ChatterId();
            _chatterInConversationId = new ChatterId();
            _chatterNotInConversationId = new ChatterId();

            _conversationRepository = Substitute.For<IConversationRepository>();
            _conversationCreationValidator = new();

            _subject = new(
                _conversationRepository,
                _conversationCreationValidator);

            AddUserToHttpContext();
        }

        [Fact]
        public async Task CreateConversation_GivenInvalidInput_ReturnsBadRequest()
        {
            //Arrange
            ConversationCreationDto invalidInput = new()
            {
                ConversationMemberIds = new List<Guid>() { Guid.NewGuid() },
                Title = "",
                Description = "ValidDescription",
            };

            //Act
            IActionResult result = await _subject.CreateConversation(invalidInput);

            //Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task CreateConversation_GivenValidInput_Returns201()
        {
            //Arrange
            ConversationCreationDto validInput = new()
            {
                ConversationMemberIds = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid() },
                Title = "ValidTitle",
                Description = "ValidDescription",
            };

            //Act
            IActionResult result = await _subject.CreateConversation(validInput);

            //Assert
            ((StatusCodeResult) result).StatusCode.Should().Be(201);
        }

        [Fact]
        public async Task LeaveConversation_GivenExistingConversationId_Returns201()
        {
            //Arrange
            Conversation conversation = Conversation.Create(
                _currentChatterId,
                new List<ChatterId>() { _currentChatterId, new ChatterId() },
                "Title");

            _conversationRepository.GetByIdAsync(_currentChatterId, conversation.Id).Returns(conversation);

            //Act
            IActionResult result = await _subject.LeaveConversation(conversation.Id.Value);

            //Assert
            ((StatusCodeResult) result).StatusCode.Should().Be(201);
        }

        [Fact]
        public async Task AddMemberToConversation_WhenCurrentChatterIsNotOwner_Returns403()
        {
            //Arrange
            Conversation conversation = Conversation.Create(
                _chatterInConversationId,
                new List<ChatterId> { _currentChatterId, _chatterInConversationId },
                "Title");

            _conversationRepository
                .GetByIdAsync(_currentChatterId, conversation.Id)
                .Returns(conversation);

            //Act
            IActionResult result = await _subject
                .AddMemberToConversation(conversation.Id.Value, _chatterNotInConversationId.Value);

            //Assert
            ((StatusCodeResult) result).StatusCode.Should().Be(403);
        }

        [Fact]
        public async Task AddMemberToConversation_AddingChatterWhoIsAlreadyAMember_Returns403()
        {
            //Arrange
            Conversation conversation = Conversation.Create(
                _currentChatterId,
                new List<ChatterId> { _currentChatterId, _chatterInConversationId },
                "Title");

            _conversationRepository
                .GetByIdAsync(_currentChatterId, conversation.Id)
                .Returns(conversation);

            //Act
            IActionResult result = await _subject
                .AddMemberToConversation(conversation.Id.Value, _chatterInConversationId.Value);

            //Assert
            ((StatusCodeResult) result).StatusCode.Should().Be(403);
        }

        [Fact]
        public async Task AddMemberToConversation_AddingNewMember_Returns201()
        {
            //Arrange
            Conversation conversation = Conversation.Create(
                _currentChatterId,
                new List<ChatterId> { _currentChatterId, _chatterInConversationId },
                "Title");

            _conversationRepository
                .GetByIdAsync(_currentChatterId, conversation.Id)
                .Returns(conversation);

            //Act
            IActionResult result = await _subject
                .AddMemberToConversation(conversation.Id.Value, _chatterNotInConversationId.Value);

            //Assert
            ((StatusCodeResult) result).StatusCode.Should().Be(201);
        }




        private void AddUserToHttpContext()
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, _currentChatterId.ToString()),
                new Claim(ClaimTypes.Name, "userName"),
                new Claim(ClaimTypes.Email, "user@email.com")

            };
            ClaimsIdentity identity = new(claims);
            ClaimsPrincipal principal = new(identity);
            _subject.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = principal
                }
            };
        }

    }
}
