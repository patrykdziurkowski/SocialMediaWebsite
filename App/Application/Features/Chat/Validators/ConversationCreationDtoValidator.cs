using Application.Features.Chat.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Chat.Validators
{
    public class ConversationCreationDtoValidator : AbstractValidator<ConversationCreationDto>
    {
        public ConversationCreationDtoValidator()
        {
            RuleFor(p => p.Title)
                .MustBeValidConversationTitle();

            RuleFor(p => p.Description)
                .MustBeValidConversationDescription();
        }
    }
}
