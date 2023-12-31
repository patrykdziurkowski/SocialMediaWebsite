﻿using Application.Features.Chatter;
using Application.Features.Conversations.Commands;
using Application.Features.Conversations.Dtos;
using Application.Features.Conversations.Interfaces;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Application.Features.Conversations
{
    [Authorize]
    public class ConversationController : Controller
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IValidator<StartConversationModel> _conversationCreationValidator;
        private readonly IValidator<PostMessageModel> _postMessageValidator;
        private readonly IPostMessageCommand _postMessageCommand;

        public ConversationController(
            IConversationRepository conversationRepository,
            IValidator<StartConversationModel> conversationCreationValidator,
            IValidator<PostMessageModel> postMessageValidator,
            IPostMessageCommand postMessageCommand)
        {
            _conversationRepository = conversationRepository;
            _conversationCreationValidator = conversationCreationValidator;
            _postMessageValidator = postMessageValidator;
            _postMessageCommand = postMessageCommand;
        }

        [HttpGet]
        [Route("Chat")]
        [Route("Conversations")]
        [Route("Messages")]
        public async Task<IActionResult> Index()
        {
            ChatterId chatterId = GetCurrentChatterId();

            IEnumerable<Conversation> conversations = await _conversationRepository.GetAllAsync(chatterId);

            return Ok(conversations);
        }

        [HttpPost]
        [Route("Conversations")]
        public async Task<IActionResult> StartConversation(StartConversationModel input)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            ValidationResult result = _conversationCreationValidator
                .Validate(input);
            if (!result.IsValid)
            {
                result.Errors.ForEach(f => ModelState.AddModelError(f.PropertyName, f.ErrorMessage));
                return ValidationProblem(ModelState);
            }

            Conversation createdConversation = Conversation.Start(
                GetCurrentChatterId(),
                input.ConversationMemberIds!.Select(guid => new ChatterId(guid)).ToList(),
                input.Title!,
                input.Description);
            await _conversationRepository.SaveAsync(createdConversation);

            return Created($"Conversations/{createdConversation.Id.Value}", new ConversationDto(createdConversation));
        }

        [HttpPatch]
        [Route("Conversations")]
        public async Task<IActionResult> LeaveConversation(Guid conversationId)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            ChatterId chatterId = GetCurrentChatterId();
            ConversationId conversationToLeaveId = new(conversationId);

            Conversation conversationToLeave = await _conversationRepository.GetByIdAsync(chatterId, conversationToLeaveId);
            conversationToLeave.Leave(chatterId);
            await _conversationRepository.SaveAsync(conversationToLeave);

            return Ok();
        }


        [HttpPost]
        [Route("Conversations/{conversationId}/Members")]
        public async Task<IActionResult> AddMemberToConversation(
            Guid conversationId,
            Guid chatterToAddId)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            ChatterId chatterId = GetCurrentChatterId();

            Conversation conversation = await _conversationRepository
                .GetByIdAsync(chatterId, new ConversationId(conversationId));
            Result result = conversation.AddMember(GetCurrentChatterId(), new ChatterId(chatterToAddId));
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
                return ValidationProblem(ModelState);
            }

            ChatterId chatterId = GetCurrentChatterId();

            Conversation conversation = await _conversationRepository
                .GetByIdAsync(chatterId, new ConversationId(conversationId));
            Result result = conversation.KickMember(chatterId, new ChatterId(chatterToKickId));
            if (result.IsFailed)
            {
                return new StatusCodeResult(403);
            }
            await _conversationRepository.SaveAsync(conversation);

            return Ok();
        }

        [HttpPost]
        [Route("Conversations/{conversationId}/Posts")]
        public async Task<IActionResult> PostMessage(
            Guid conversationId,
            PostMessageModel input)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            ValidationResult validationResult = _postMessageValidator.Validate(input);
            if (!validationResult.IsValid)
            {
                validationResult.Errors.ForEach(f => ModelState.AddModelError(f.PropertyName, f.ErrorMessage));
                return ValidationProblem(ModelState);
            }

            Message message = await _postMessageCommand.Handle(
                GetCurrentChatterId(),
                new ConversationId(conversationId),
                input);

            return Created("Conversation/{conversationId}/Posts", message);
        }

        [HttpDelete]
        [Route("Conversations/{conversationId}/Posts")]
        public async Task<IActionResult> DeleteMessage(
            Guid conversationId,
            Guid messageId)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            ChatterId currentChatterId = GetCurrentChatterId();

            Conversation conversation = await _conversationRepository.GetByIdAsync(
                currentChatterId,
                new ConversationId(conversationId));
            conversation.DeleteMessage(
                currentChatterId,
                new MessageId(messageId));
            await _conversationRepository.SaveAsync(conversation);

            return Ok();
        }


        private ChatterId GetCurrentChatterId()
        {
            string chatterIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            return new ChatterId(Guid.Parse(chatterIdClaim));
        }

    }
}
