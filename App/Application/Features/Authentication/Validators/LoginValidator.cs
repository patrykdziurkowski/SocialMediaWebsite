using Application.Features.Authentication.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
