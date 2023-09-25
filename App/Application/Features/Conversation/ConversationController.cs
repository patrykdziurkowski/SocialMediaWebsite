using Application.Features.Chat.Dtos;
using Application.Features.Chat.Interfaces;
using Application.Features.Chatter;
using Application.Features.Conversation.Dtos;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Application.Features.Chat
{
    [Authorize]
    public class ConversationController : Controller
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IValidator<StartConversationModel> _conversationCreationValidator;
        private readonly IValidator<PostMessageModel> _postMessageValidator;

        public ConversationController(
            IConversationRepository conversationRepository,
            IValidator<StartConversationModel> conversationCreationValidator,
            IValidator<PostMessageModel> postMessageValidator)
        {
            _conversationRepository = conversationRepository;
            _conversationCreationValidator = conversationCreationValidator;
            _postMessageValidator = postMessageValidator;
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
        public async Task<IActionResult> StartConversation(StartConversationModel input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FluentValidation.Results.ValidationResult result = _conversationCreationValidator
                .Validate(input);
            if (!result.IsValid)
            {
                result.Errors.ForEach(f => ModelState.AddModelError(f.PropertyName, f.ErrorMessage));
                return BadRequest(ModelState);
            }

            ChatterId chatterId = GetCurrentUserId();

            Conversation createdConversation = Conversation.Start(
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
                return BadRequest(ModelState);
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
                return BadRequest(ModelState);
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
                return BadRequest(ModelState);
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

        [HttpPost]
        [Route("Conversations/{conversationId}/Posts")]
        public async Task<IActionResult> PostMessage(
            Guid conversationId,
            PostMessageModel input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ValidationResult validationResult = _postMessageValidator.Validate(input);
            if (!validationResult.IsValid)
            {
                validationResult.Errors.ForEach(f => ModelState.AddModelError(f.PropertyName, f.ErrorMessage));
                return BadRequest(ModelState);
            }

            ChatterId currentChatterId = GetCurrentUserId();
            MessageId? replyMessageId = null;
            if (input.ReplyMessageId is not null)
            {
                replyMessageId = new MessageId((Guid) input.ReplyMessageId!);
            }

            Conversation conversation = await _conversationRepository.GetByIdAsync(
                currentChatterId,
                new ConversationId(conversationId));
            conversation.PostMessage(
                currentChatterId,
                input.Text!,
                replyMessageId);
            await _conversationRepository.SaveAsync(conversation);

            return new StatusCodeResult(201);
        }

        [HttpDelete]
        [Route("Conversations/{conversationId}/Posts")]
        public async Task<IActionResult> DeleteMessage(
            Guid conversationId,
            Guid messageId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ChatterId currentChatterId = GetCurrentUserId();

            Conversation conversation = await _conversationRepository.GetByIdAsync(
                currentChatterId,
                new ConversationId(conversationId));
            conversation.DeleteMessage(
                currentChatterId,
                new MessageId(messageId));
            await _conversationRepository.SaveAsync(conversation);

            return Ok();
        }


        private ChatterId GetCurrentUserId()
        {
            string chatterIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            return new ChatterId(Guid.Parse(chatterIdClaim));
        }

    }
}
