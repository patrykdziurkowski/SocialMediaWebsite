using Application.Features.Authentication.Models;
using FluentValidation;

namespace Application.Features.Authentication.Validators
{
    public class LoginValidator : AbstractValidator<UserLoginModel>
    {
        public LoginValidator()
        {
            RuleFor(x => x.UserName).
                MustBeValidUserName();

            RuleFor(x => x.Password)
                .MustBeValidPassword();
        }
    }
}
