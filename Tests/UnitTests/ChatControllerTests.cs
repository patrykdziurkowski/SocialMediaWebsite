using Application.Features.Authentication;
using Application.Features.Chat;
using Application.Features.Chat.Dtos;
using Application.Features.Chat.Interfaces;
using Application.Features.Chat.Validators;
using Castle.Components.DictionaryAdapter.Xml;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.UnitTests
{
    public class ChatControllerTests
    {
        private readonly ChatController _subject;

        private readonly IChatRepository _chatRepository;
        private readonly ConversationCreationDtoValidator _conversationCreationValidator;

        public ChatControllerTests()
        {
            _chatRepository = Substitute.For<IChatRepository>();
            _conversationCreationValidator = new();

            _subject = new(
                _chatRepository,
                _conversationCreationValidator);

            AddUserToHttpContext();
        }

        [Fact]
        public async Task CreateConversation_GivenInvalidInput_ReturnsBadRequest()
        {
            //Arrange
            ConversationCreationDto invalidInput = new()
            { 
                ConversationMemberIds = new List<int>() { 1 },
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
            Chat usersChat = new(1, new List<Conversation>());
            ConversationCreationDto validInput = new()
            {
                ConversationMemberIds = new List<int>() { 1, 2 },
                Title = "ValidTitle",
                Description = "ValidDescription",
            };  

            _chatRepository.GetAsync(1).Returns(usersChat);

            //Act
            IActionResult result = await _subject.CreateConversation(validInput);

            //Assert
            ((StatusCodeResult) result).StatusCode.Should().Be(201);
        }



        private void AddUserToHttpContext()
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
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
