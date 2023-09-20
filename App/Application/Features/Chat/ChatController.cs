using Application.Features.Chat.Dtos;
using Application.Features.Chat.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Chat
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IChatRepository _chatRepository;
        private readonly IValidator<ConversationCreationDto> _conversationCreationValidator;

        public ChatController(
            IChatRepository chatRepository,
            IValidator<ConversationCreationDto> conversationCreationValidator)
        {
            _chatRepository = chatRepository;
            _conversationCreationValidator = conversationCreationValidator;
        }

        [HttpGet]
        [Route("Chat")]
        [Route("Conversations")]
        [Route("Messages")]
        public async Task<IActionResult> Index()
        {
            ChatterId chatterId = GetCurrentUserId();

            Chat chat = await _chatRepository.GetAsync(chatterId);

            return Ok(chat);
        }

        [HttpPost]
        [Route("Conversations")]
        public async Task<IActionResult> CreateConversation(ConversationCreationDto input)
        {
            FluentValidation.Results.ValidationResult result = _conversationCreationValidator
                .Validate(input);
            if (!result.IsValid || !ModelState.IsValid)
            {
                return BadRequest();
            }

            ChatterId chatterId = GetCurrentUserId();

            Chat chat = await _chatRepository.GetAsync(chatterId);
            chat.CreateConversation(
                input.ConversationMemberIds!,
                input.Title!,
                input.Description);
            await _chatRepository.SaveAsync(chat);

            return new StatusCodeResult(201);
        }

        [HttpPatch]
        [Route("Conversations")]
        public async Task<IActionResult> LeaveConversation(Guid conversationid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ChatterId chatterId = GetCurrentUserId();

            Chat chat = await _chatRepository.GetAsync(chatterId);
            chat.LeaveConversation(conversationid);
            await _chatRepository.SaveAsync(chat);

            return new StatusCodeResult(201);
        }

        private ChatterId GetCurrentUserId()
        {
            string chatterIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            return new ChatterId(Guid.Parse(chatterIdClaim));
        }

    }
}
