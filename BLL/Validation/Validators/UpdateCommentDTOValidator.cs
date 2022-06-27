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
    /// Validator of UpdateCommentDTO
    /// </summary>
    public class UpdateCommentDTOValidator: AbstractValidator<UpdateCommentDTO>
    {
        public UpdateCommentDTOValidator()
        {
            RuleFor(c => c.Content).NotEmpty().WithMessage("{PropertyName} must not be empty");
        }
    }
}
