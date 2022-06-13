using Core.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Validation.Validators
{
    public class TextMaterialValidator : AbstractValidator<TextMaterial>
    {
        public TextMaterialValidator()
        {
            RuleFor(tm => tm.Title).Length(5, 100);
            RuleFor(tm => tm.Content).NotEmpty();
        }
    }
}
