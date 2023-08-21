using Application.Features.Authentication.Models;
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
