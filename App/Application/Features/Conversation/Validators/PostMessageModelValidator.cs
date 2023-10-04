using Application.Features.Conversations.Dtos;
using FluentValidation;

namespace Application.Features.Conversations.Validators
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
