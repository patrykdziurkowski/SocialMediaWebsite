using Application.Features.Authentication.Models;
using FluentValidation;

namespace Application.Features.Authentication.Validators
{
    public class RegisterValidator : AbstractValidator<UserRegisterModel>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.UserName)
                .MustBeValidUserName();

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Incorrect email format");

            RuleFor(x => x.Password)
                .MustBeValidPassword();
        
        }

    }
}
