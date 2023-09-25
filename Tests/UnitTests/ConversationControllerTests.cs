using Application.Features.Chat;
using Application.Features.Chat.Dtos;
using Application.Features.Chat.Interfaces;
using Application.Features.Chat.Validators;
using Application.Features.Chatter;
using Application.Features.Conversation.Dtos;
using Application.Features.Conversation.Validators;
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
        private readonly CreateConversationModelValidator _conversationCreationValidator;
        private readonly PostMessageModelValidator _postMessageValidator;

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
            _postMessageValidator = new();

            _subject = new(
                _conversationRepository,
                _conversationCreationValidator,
                _postMessageValidator);

            AddUserToHttpContext();
        }

        [Fact]
        public async Task CreateConversation_GivenInvalidInput_ReturnsBadRequest()
        {
            //Arrange
            CreateConversationModel invalidInput = new()
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
            CreateConversationModel validInput = new()
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

        [Fact]
        public async Task KickMemberFromConversation_WhenCurrentChatterIsNotOwner_Returns403()
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
                .KickMemberFromConversation(conversation.Id.Value, _chatterInConversationId.Value);

            //Assert
            ((StatusCodeResult) result).StatusCode.Should().Be(403);
        }

        [Fact]
        public async Task KickMemberFromConversation_WhenKickedChatterWasNotInConversation_Throws()
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
            Func<Task> kick = async () =>
            {
                await _subject
                .KickMemberFromConversation(conversation.Id.Value, _chatterNotInConversationId.Value);
            };

            //Assert
            await kick.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task KickMemberFromConversation_Returns201_WhenSuccessfulyKicked()
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
                .KickMemberFromConversation(conversation.Id.Value, _chatterInConversationId.Value);

            //Assert
            ((StatusCodeResult) result).StatusCode.Should().Be(201);
        }

        [Fact]
        public async Task PostMessage_Returns400_WhenInvalidInput()
        {
            //Arrange
            PostMessageModel input = new()
            {
                Text = "",
                ReplyMessageId = null
            };

            //Act
            IActionResult result = await _subject.PostMessage(Guid.Empty, input);

            //Assert
            ((StatusCodeResult) result).StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task PostMessage_Returns201_WhenSuccessfulyCreated()
        {
            //Arrange
            PostMessageModel input = new()
            {
                Text = "Some valid message!1",
                ReplyMessageId = null
            };

            Conversation conversation = Conversation.Create(
                _currentChatterId,
                new List<ChatterId> { _currentChatterId, _chatterNotInConversationId },
                "Title");

            _conversationRepository
                .GetByIdAsync(_currentChatterId, conversation.Id)
                .Returns(conversation);

            //Act
            IActionResult result = await _subject.PostMessage(conversation.Id.Value, input);

            //Assert
            ((StatusCodeResult) result).StatusCode.Should().Be(201);
        }

        [Fact]
        public async Task DeleteMessage_Returns200_UponSuccessfulDeletion()
        {
            //Arrange
            Conversation conversation = Conversation.Create(
                _currentChatterId,
                new List<ChatterId> { _currentChatterId, _chatterNotInConversationId },
                "Title");
            conversation.PostMessage(_currentChatterId, "Text");

            _conversationRepository
                .GetByIdAsync(_currentChatterId, conversation.Id)
                .Returns(conversation);

            Guid inputMessageId = conversation.LoadedMessages.Single().Id.Value;

            //Act
            IActionResult result = await _subject.DeleteMessage(conversation.Id.Value, inputMessageId);

            //Assert
            ((StatusCodeResult) result).StatusCode.Should().Be(200);
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
