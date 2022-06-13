using Core.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Validation.Validators
{
    public class UpdateTextMaterialDTOValidator : AbstractValidator<UpdateTextMaterialDTO>
    {
        public UpdateTextMaterialDTOValidator()
        {
            RuleFor(tm => tm.Title)
               .NotEmpty().WithMessage("{PropertyName} must not be empty")
               .Length(5, 100).WithMessage("{PropertyName} must be 5 to 100 characters long")
               .Must(IsValidTitle).WithMessage("{PropertyName} must be all letters");

            RuleFor(tm => tm.Content)
                .NotEmpty().WithMessage("{PropertyName} must not be empty");

            RuleFor(tm => tm.CategoryTitle)
                .NotEmpty().WithMessage("{PropertyName} must not be empty");

            RuleFor(tm => tm.ApprovalStatusId)
                .InclusiveBetween(0, 2).WithMessage("{PropertyName} must be valid");

            RuleFor(tm => tm.AuthorId)
                .NotEmpty().WithMessage("{PropertyName} must not be empty");
        }

        private bool IsValidTitle(string title)
        {
            return title.All(Char.IsLetter);
        }
    }
}
