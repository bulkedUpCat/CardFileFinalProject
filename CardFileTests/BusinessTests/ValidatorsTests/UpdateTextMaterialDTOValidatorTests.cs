using BLL.Validation.Validators;
using Core.DTOs;
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
    public class UpdateTextMaterialDTOValidatorTests
    {
        private UpdateTextMaterialDTOValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new UpdateTextMaterialDTOValidator();
        }

        [Test]
        public void GivenEmptyTitle_ShouldHaveValidationError()
        {
            var textMaterial = new UpdateTextMaterialDTO { Title = "", Content = "something", AuthorId = "1" };
            var result = _validator.TestValidate(textMaterial);
            result.ShouldHaveValidationErrorFor(tm => tm.Title);
        }

        [Test]
        public void GivenTitleLessThanFiveSymbols_ShouldHaveValidationError()
        {
            var textMaterial = new UpdateTextMaterialDTO { Title = "abc", Content = "something", AuthorId = "1" };
            var result = _validator.TestValidate(textMaterial);
            result.ShouldHaveValidationErrorFor(tm => tm.Title);
        }

        [Test]
        public void GivenEmptyContent_ShouldHaveValidationError()
        {
            var textMaterial = new UpdateTextMaterialDTO { Title = "New one", Content = "", AuthorId = "1" };
            var result = _validator.TestValidate(textMaterial);
            result.ShouldHaveValidationErrorFor(tm => tm.Content);
        }

        [Test]
        public void GivenEmptyAuthorId_ShouldHaveValidationError()
        {
            var textMaterial = new UpdateTextMaterialDTO { Title = "New one", Content = "something", AuthorId = "" };
            var result = _validator.TestValidate(textMaterial);
            result.ShouldHaveValidationErrorFor(tm => tm.AuthorId);
        }
    }
}
