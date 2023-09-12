using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class FluentValidationCustomRules
    {
        public static IRuleBuilderOptions<T, string> MustContainADigit<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must((property) =>
                {
                    if (property is null)
                    {
                        return false;
                    }

                    bool containsDigit = property.Any(character => char.IsDigit(character));
                    return containsDigit;
                });
        }

        public static IRuleBuilderOptions<T, string> MustContainAnUppercase<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must((property) =>
                {
                    if (property is null)
                    {
                        return false;
                    }

                    bool containsDigit = property.Any(character => char.IsUpper(character));
                    return containsDigit;
                });
        }

        public static IRuleBuilderOptions<T, string> MustContainALowercase<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must((property) =>
                {
                    if (property is null)
                    {
                        return false;
                    }

                    bool containsDigit = property.Any(character => char.IsLower(character));
                    return containsDigit;
                });
        }

        public static IRuleBuilderOptions<T, string> MustContainOnlyAlphanumeric<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must((property) =>
                {
                    if (property is null)
                    {
                        return true;
                    }

                    bool isAlphanumeric = property.All(character => char.IsLetterOrDigit(character));
                    return isAlphanumeric;
                });
        }

        public static IRuleBuilderOptions<T, string> MustContainASpecialCharacter<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            const string SpecialCharacters = "~`!@#$%^&*()_-+=[]{}\\|:;\"\'<>,./?";

            return ruleBuilder
                .Must((property) =>
                {
                    if (property is null)
                    {
                        return false;
                    }

                    bool containsSpecialCharacter = property.Any(character => SpecialCharacters.Contains(character));
                    return containsSpecialCharacter;
                });
        }

        public static IRuleBuilderOptions<T, string?> MustBeValidUserName<T>(
            this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(20)
                .MustContainOnlyAlphanumeric()
                .WithMessage("UserName must be between 6 and 20 alphanumeric characters");
        }

        public static IRuleBuilderOptions<T, string?> MustBeValidPassword<T>(
            this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .MinimumLength(8)
                .MustContainADigit()
                .MustContainAnUppercase()
                .MustContainALowercase()
                .MustContainASpecialCharacter()
                .WithMessage("Password must be a minimum of 8 characters with at least one digit, uppercase letter, lowercase letter, and a special character.");
        }

        public static IRuleBuilderOptions<T, string?> MustBeValidConversationTitle<T>(
            this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(20)
                .WithMessage("Title must be between 2 and 20 characters");
        }


    }
}
