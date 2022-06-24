using BLL.Validation.Validators;
using FluentValidation.TestHelper;
using Core.DTOs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFileTests.BusinessTests.ValidatorsTests
{
    public class CreateTextMaterialDTOValidatorTests
    {
        private CreateTextMaterialDTOValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new CreateTextMaterialDTOValidator();
        }

        [TestCase("")]
        [TestCase("abbb")]
        public void GivenInvalidTitle_ShouldHaveValidationError(string title)
        {
            var textMaterial = new CreateTextMaterialDTO { Title = title };
            var result = _validator.TestValidate(textMaterial);
            result.ShouldHaveValidationErrorFor(tm => tm.Title);
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase(null)]
        public void GivenInvalidContent_ShouldHaveValidationError(string content)
        {
            var textMaterial = new CreateTextMaterialDTO { Content = content };
            var result = _validator.TestValidate(textMaterial);
            result.ShouldHaveValidationErrorFor(tm => tm.Content);
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase(null)]
        public void GivenInvalidCategoryTitle_ShouldHaveValidationError(string categoryTitle)
        {
            var textMaterial = new CreateTextMaterialDTO { CategoryTitle = categoryTitle };
            var result = _validator.TestValidate(textMaterial);
            result.ShouldHaveValidationErrorFor(tm => tm.CategoryTitle);
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase(null)]
        public void GivenInvalidAuthorId_ShouldHaveValidationError(string authorId)
        {
            var textMaterial = new CreateTextMaterialDTO { AuthorId = authorId };
            var result = _validator.TestValidate(textMaterial);
            result.ShouldHaveValidationErrorFor(tm => tm.AuthorId);
        }

    }
}
