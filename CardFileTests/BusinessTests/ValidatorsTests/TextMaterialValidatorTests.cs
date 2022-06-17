using BLL.Validation.Validators;
using Core.Models;
using FluentValidation.TestHelper;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFileTests.BusinessTests.ValidatorsTests
{
    [TestFixture]
    public class TextMaterialValidatorTests
    {
        private TextMaterialValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new TextMaterialValidator();
        }

        [TestCase("abc")]
        [TestCase("test")]
        public void GivenInvalidTitle_ShouldHaveValidationError(string title)
        {
            var textMaterial = new TextMaterial { Title = title };
            var result = _validator.TestValidate(textMaterial);
            result.ShouldHaveValidationErrorFor(tm => tm.Title);
        }

        [TestCase("title")]
        [TestCase("valid title")]
        public void GivenValidTitle_ShouldNotHaveValidationError(string title)
        {
            var textMaterial = new TextMaterial { Title= title };
            var result = _validator.TestValidate(textMaterial);
            result.ShouldNotHaveValidationErrorFor(tm => tm.Title);
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase(" ")]
        public void GivenEmptyContent_ShouldHaveValidationError(string content)
        {
            var textMaterial = new TextMaterial { Content = content };
            var result = _validator.TestValidate(textMaterial);
            result.ShouldHaveValidationErrorFor(tm => tm.Content);
        }

        [TestCase("valid content")]
        [TestCase("test content")]
        public void GivenNonEmptyContent_ShouldNotHaveValidationError(string content)
        {
            var textMaterial = new TextMaterial { Content= content };
            var result = _validator.TestValidate(textMaterial);
            result.ShouldNotHaveValidationErrorFor(tm => tm.Content);
        }
    }
}
