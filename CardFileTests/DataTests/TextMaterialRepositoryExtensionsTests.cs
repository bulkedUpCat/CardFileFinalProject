using Core.Models;
using DAL.Contexts;
using DAL.Extensions;
using DAL.Repositories;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace CardFileTests.DataTests
{
    public class TextMaterialRepositoryExtensionsTests
    {
        [TestCase("article")]
        [TestCase("se")]
        [TestCase("")]
        public void TextMaterialRepositoryExtensions_SearchByTitle_ReturnsCorrectValues(string title)
        {
            // Arrange
            var expected = ExpectedTextMaterials.Where(tm => tm.Title.ToLower().Contains(title.Trim().ToLower())).ToList();
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());

            // Act
            var actual = context.TextMaterials.SearchByTitle(title).ToList();

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(new TextMaterialEqualityComparer()), message: "SearchByTitle extension method works incorrectly");
        }

        [TestCase("2000-1-1","2010-1-1")]
        [TestCase("2004-1-1","2005-1-1")]
        [TestCase("2010-1-1","2022-1-1")]
        public void TextMaterialRepositoryExtensions_FilterByDatePublished_ReturnsCorrectValues(string fromDate, string toDate)
        {
            // Arrange
            var expected = ExpectedTextMaterials.Where(tm =>
                DateTime.Compare(tm.DatePublished.Date, DateTime.Parse(fromDate)) >= 0 &&
                DateTime.Compare(tm.DatePublished.Date, DateTime.Parse(toDate)) <= 0);
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());

            // Act
            var actual = context.TextMaterials.FilterByDatePublished(fromDate.ToString(), toDate.ToString()).ToList();

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(new TextMaterialEqualityComparer()), message: "FilterByDatePublished extension method works incorrectly");
        }

        [TestCase(new int[] { 1, 2 })]
        [TestCase(new int[] { 0, 1 })]
        [TestCase(new int[] { 0, 1, 2})]
        public void TextMaterialRepositoryExtensions_FilterByApprovalStatus_ReturnsCorrectValues(int[] approvalStatus)
        {
            // Arrange
            var expected = ExpectedTextMaterials.Where(tm => approvalStatus.Contains((int)tm.ApprovalStatus)).ToList();
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());

            // Act
            var actual = context.TextMaterials.FilterByApprovalStatus(approvalStatus.ToList());

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(new TextMaterialEqualityComparer()), message: "FilterByApprovalStatus extension method works incorrectly");
        }

        [TestCase("title asc")]
        [TestCase("datePublished desc, title asc")]
        public void TextMaterialRepositoryExtensions_Sort_ReturnsOrderedValues(string orderBy)
        {
            // Arrange
            var expected = ExpectedTextMaterials.AsQueryable().OrderBy(orderBy);
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());

            // Act
            var actual = context.TextMaterials.Sort(orderBy);

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(new TextMaterialEqualityComparer()), message: "Sort extension method works incorrectly");
        }
         

        private static IEnumerable<TextMaterial> ExpectedTextMaterials =
            new[]
            {
                new TextMaterial { Id = 1, AuthorId = "1", ApprovalStatus = ApprovalStatus.Pending, Content = "firstContent", Title = "firstArticle", TextMaterialCategoryId = 1, TextMaterialCategory = new TextMaterialCategory { Id = 1, Title = "First one" }, DatePublished = new DateTime(2000,3,12) },
                new TextMaterial { Id = 2, AuthorId = "2", ApprovalStatus = ApprovalStatus.Approved, Content = "secondContent", Title = "secondArticle", TextMaterialCategoryId = 1, TextMaterialCategory = new TextMaterialCategory { Id = 1, Title = "First one" }, DatePublished = new DateTime(2003, 4,23) },
                new TextMaterial { Id = 3, AuthorId = "2", ApprovalStatus = ApprovalStatus.Approved, Content = "thirdContent", Title = "thirdArticle", TextMaterialCategoryId = 2, TextMaterialCategory = new TextMaterialCategory { Id = 2, Title = "Second one" }, DatePublished = new DateTime(2004,1,1) }
            };
    }
}
