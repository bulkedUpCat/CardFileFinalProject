using Core.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL.Validation.Validators
{
    public class UserLoginDTOValidator : AbstractValidator<UserLoginDTO>
    {
        public UserLoginDTOValidator()
        {
            RuleFor(u => u.Password)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} must not be empty")
                .MinimumLength(4).WithMessage("{PropertyName} must be at least 6 characters long")
                .Matches(@"[\!\?\@\*\.]+").WithMessage("{PropertyName} must contain at least one symbol")
                .Must(HasDigit).WithMessage("{PropertyName} must contain at least one digit")
                .Must(HasLowerCaseLetter).WithMessage("{PropertyName} must contain at least one lowercase letter");

            RuleFor(u => u.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} must not be empty")
                .EmailAddress().WithMessage("{PropertyName} must be valid");
        }

        private bool HasDigit(string password)
        {
            return password.Any(Char.IsDigit);
        }

        private bool HasLowerCaseLetter(string password)
        {
            return password.Any(Char.IsLower);
        }
    }
}
