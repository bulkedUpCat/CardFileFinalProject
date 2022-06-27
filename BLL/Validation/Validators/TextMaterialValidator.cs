using Core.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Validation.Validators
{
    /// <summary>
    /// Validator of TextMaterial
    /// </summary>
    public class TextMaterialValidator : AbstractValidator<TextMaterial>
    {
        public TextMaterialValidator()
        {
            RuleFor(tm => tm.Title).Length(5, 100).WithMessage("{PropertyName} must be 5 to 100 characters long");
            RuleFor(tm => tm.Content).NotEmpty().WithMessage("{PropertyName} must not be empty");
        }
    }
}
