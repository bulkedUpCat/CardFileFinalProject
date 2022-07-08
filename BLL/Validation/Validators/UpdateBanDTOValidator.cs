using Core.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Validation.Validators
{
    public class UpdateBanDTOValidator: AbstractValidator<UpdateBanDTO>
    {
        public UpdateBanDTOValidator()
        {
            RuleFor(b => b.Reason)
                .NotEmpty().WithMessage("{PropertyName} must not be empty")
                .MaximumLength(100).WithMessage("{PropertyName} must be maximum 100 symbols long");

            RuleFor(b => b.Days)
                .InclusiveBetween(1, 100).WithMessage("{PropertyName} must be 1 to 100");

            RuleFor(b => b.UserId)
                .NotEmpty().WithMessage("{PropertyName} must not be empty");
        }
    }
}
