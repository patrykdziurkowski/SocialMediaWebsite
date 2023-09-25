using Application.Features.Conversation.Dtos;
using FluentValidation;

namespace Application.Features.Conversation.Validators
{
    public class PostMessageDtoValidator : AbstractValidator<PostMessageDto>
    {
        public PostMessageDtoValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty()
                .MaximumLength(500);
        }
    }
}
