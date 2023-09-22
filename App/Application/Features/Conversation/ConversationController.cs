using Application.Features.Chat.Dtos;
using Application.Features.Chat.Interfaces;
using Application.Features.Chatter;
using FluentResults;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Application.Features.Chat
{
    [Authorize]
    public class ConversationController : Controller
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IValidator<ConversationCreationDto> _conversationCreationValidator;

        public ConversationController(
            IConversationRepository conversationRepository,
            IValidator<ConversationCreationDto> conversationCreationValidator)
        {
            _conversationRepository = conversationRepository;
            _conversationCreationValidator = conversationCreationValidator;
        }

        [HttpGet]
        [Route("Chat")]
        [Route("Conversations")]
        [Route("Messages")]
        public async Task<IActionResult> Index()
        {
            ChatterId chatterId = GetCurrentUserId();

            IEnumerable<Conversation> conversations = await _conversationRepository.GetAllAsync(chatterId);

            return Ok(conversations);
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

            Conversation createdConversation = Conversation.Create(
                chatterId,
                input.ConversationMemberIds!.Select(guid => new ChatterId(guid)).ToList(),
                input.Title!,
                input.Description);
            await _conversationRepository.SaveAsync(createdConversation);

            return new StatusCodeResult(201);
        }

        [HttpPatch]
        [Route("Conversations")]
        public async Task<IActionResult> LeaveConversation(Guid conversationId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ChatterId chatterId = GetCurrentUserId();
            ConversationId conversationToLeaveId = new(conversationId);

            Conversation conversationToLeave = await _conversationRepository.GetByIdAsync(chatterId, conversationToLeaveId);
            conversationToLeave.Leave(chatterId);
            await _conversationRepository.SaveAsync(conversationToLeave);

            return new StatusCodeResult(201);
        }


        [HttpPost]
        [Route("Conversations/{conversationId}/Members")]
        public async Task<IActionResult> AddMemberToConversation(
            Guid conversationId,
            Guid chatterToAddId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ChatterId chatterId = GetCurrentUserId();

            Conversation conversation = await _conversationRepository
                .GetByIdAsync(chatterId, new ConversationId(conversationId));

            Result result = conversation.AddMember(chatterId, new ChatterId(chatterToAddId));
            if (result.IsFailed)
            {
                return new StatusCodeResult(403);
            }

            await _conversationRepository.SaveAsync(conversation);
            return new StatusCodeResult(201);
        }

        [HttpDelete]
        [Route("Conversations/{conversationId}/Members")]
        public async Task<IActionResult> KickMemberFromConversation(
            Guid conversationId,
            Guid chatterToKickId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ChatterId chatterId = GetCurrentUserId();

            Conversation conversation = await _conversationRepository
                .GetByIdAsync(chatterId, new ConversationId(conversationId));

            Result result = conversation.KickMember(chatterId, new ChatterId(chatterToKickId));
            if (result.IsFailed)
            {
                return new StatusCodeResult(403);
            }

            await _conversationRepository.SaveAsync(conversation);
            return new StatusCodeResult(201);
        }





        private ChatterId GetCurrentUserId()
        {
            string chatterIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            return new ChatterId(Guid.Parse(chatterIdClaim));
        }

    }
}
