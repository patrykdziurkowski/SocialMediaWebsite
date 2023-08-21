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

    }
}
