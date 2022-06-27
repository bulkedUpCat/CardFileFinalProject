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
    /// Validator of CreateTextMaterialCategoryDTO
    /// </summary>
    public class CreateTextMaterialCategoryDTOValidator: AbstractValidator<CreateTextMaterialCategoryDTO> 
    {
        public CreateTextMaterialCategoryDTOValidator()
        {
            RuleFor(tmc => tmc.Title).NotEmpty().WithMessage("{PropertyName} must not be empty");
        }
    }
}
