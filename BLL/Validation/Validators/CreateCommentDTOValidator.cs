using Core.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Validation.Validators
{
    public class CreateCommentDTOValidator: AbstractValidator<CreateCommentDTO>
    {
        public CreateCommentDTOValidator()
        {
            RuleFor(c => c.Content).NotEmpty().WithMessage("{PropertyName} must not be empty");
        }
    }
}
