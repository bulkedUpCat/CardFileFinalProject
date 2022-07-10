using Core.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Validation.Validators
{
    /// <summary>
    /// Validator of ChangeUserNameDTO
    /// </summary>
    public class ChangeUserNameDTOValidator: AbstractValidator<ChangeUserNameDTO>
    {
        public ChangeUserNameDTOValidator()
        {
            RuleFor(u => u.NewUserName)
                .NotEmpty().WithMessage("{PropretyName} must not be empty")
                .MinimumLength(4).WithMessage("{PropertyName} must be at least 4 characters long")
                .Matches(@"^[a-zA-Z]*$").WithMessage("{PropertyName} must be all letters");
        }
    }
}
