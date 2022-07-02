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
    public class UpdateCommentDTOValidatorTests
    {
        private UpdateCommentDTOValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new UpdateCommentDTOValidator();
        }

        [Test]
        public void GivenEmptyContent_ShouldHaveValidationError()
        {
            var comment = new UpdateCommentDTO { Content = "" };
            var result = _validator.TestValidate(comment);
            result.ShouldHaveValidationErrorFor(c => c.Content);
        }
    }
}
