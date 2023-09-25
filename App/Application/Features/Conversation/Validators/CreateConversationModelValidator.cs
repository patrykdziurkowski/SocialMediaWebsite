using Application.Features.Chat.Dtos;
using FluentValidation;

namespace Application.Features.Chat.Validators
{
    public class CreateConversationModelValidator : AbstractValidator<CreateConversationModel>
    {
        public CreateConversationModelValidator()
        {
            RuleFor(p => p.Title)
                .MustBeValidConversationTitle();

            RuleFor(p => p.Description)
                .MustBeValidConversationDescription();

            RuleFor(p => p.ConversationMemberIds)
                .NotEmpty()
                .Must(list => list?.Distinct().Count() > 1);
        }
    }
}
