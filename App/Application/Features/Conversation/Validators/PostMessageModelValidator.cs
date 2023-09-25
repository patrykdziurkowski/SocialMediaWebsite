using Application.Features.Conversation.Dtos;
using FluentValidation;

namespace Application.Features.Conversation.Validators
{
    public class PostMessageModelValidator : AbstractValidator<PostMessageModel>
    {
        public PostMessageModelValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty()
                .MaximumLength(500);
        }
    }
}
