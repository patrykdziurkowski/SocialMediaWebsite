using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Authentication.Validators
{
    public class RegisterValidator : AbstractValidator<UserRegisterModel>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(20)
                .MustContainOnlyAlphanumeric()
                .WithMessage("UserName must be between 6 and 20 alphanumeric characters");

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Incorrect email format");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .MustContainADigit()
                .MustContainAnUppercase()
                .MustContainALowercase()
                .MustContainASpecialCharacter()
                .WithMessage("Password must be a minimum of 8 characters with at least one digit, uppercase letter, lowercase letter, and a special character.");
        
        }

    }
}
