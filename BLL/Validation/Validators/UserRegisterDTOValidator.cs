using Core.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Validation.Validators
{
    public class UserRegisterDTOValidator : AbstractValidator<UserRegisterDTO>
    {
        public UserRegisterDTOValidator()
        {
            RuleFor(u => u.Password)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} must not be empty")
                .MinimumLength(4).WithMessage("{PropertyName} must be at least 6 characters long")
                .Matches(@"[\!\?\@\*\.]+").WithMessage("{PropertyName} must contain at least one symbol")
                .Must(HasDigit).WithMessage("{PropertyName} must contain at least one digit")
                .Must(HasLowerCaseLetter).WithMessage("{PropertyName} must contain at least one lowercase letter");

            RuleFor(u => u.ConfirmPassword)
                .NotEmpty().WithMessage("{PropertyName} must not be empty");

            RuleFor(u => u.Name)
                .NotEmpty().WithMessage("{PropretyName} must not be empty")
                .MinimumLength(4).WithMessage("{PropertyName} must be at least 4 characters long")
                .Must(IsValidName).WithMessage("{PropertyName} must be all letters");

            RuleFor(u => u.Email)
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

        private bool IsValidName(string name)
        {
            return name.All(Char.IsLetter);
        }
    }
}
