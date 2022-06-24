using Core.DTOs;
using Core.Models;
using Core.RequestFeatures;
using DAL.Contexts;
using DAL.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFileTests.DataTests
{
    [TestFixture]
    public class TextMaterialRepositoryTests
    {
        [Test]
        public async Task TextMaterialRepository_GetAsync_ReturnsAllValues()
        {
            // Arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new TextMaterialRepository(context);
            var expected = ExpectedTextMaterials.ToList();

            // Act
            var actual = await repository.GetAsync();

            // Assert
            Assert.That(actual, Is.EquivalentTo(expected).Using(new TextMaterialEqualityComparer()), message: "GetAsync method works incorrectly");
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task TextMaterialRepository_GetByIdAsync_ReturnsSingleValue(int id)
        {
            // Arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new TextMaterialRepository(context);
            var expected = ExpectedTextMaterials
                .FirstOrDefault(tm => tm.Id == id);

            // Act
            var textMaterial = await repository.GetByIdAsync(id);

            // Assert
            Assert.That(expected, Is.EqualTo(textMaterial).Using(new TextMaterialEqualityComparer()), message: "GetByIdAsync method works incorrectly");
        }

        [TestCase("1")]
        [TestCase("2")]
        public async Task TextMaterialRepository_GetByUserId_ReturnsTextMaterialsOfUser(string userId)
        {
            // Arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new TextMaterialRepository(context);
            var expected = ExpectedTextMaterials
                .Where(tm => tm.AuthorId == userId)
                .OrderByDescending(tm => tm.DatePublished)
                .ToList();

            // Act
            var actual = await repository.GetByUserId(userId, new TextMaterialParameters());

            // Assert
            Assert.That(expected, Is.EqualTo(actual).Using(new TextMaterialEqualityComparer()), message: "GetByUserId method works incorrectly");
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task TextMaterialRepository_GetByCategory_ReturnsMaterialsOfCategory(int categoryId)
        {
            // Arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new TextMaterialRepository(context);
            var expected = ExpectedTextMaterials.Where(tm => tm.TextMaterialCategoryId == categoryId);

            // Act
            var actual = await repository.GetByCategoryId(categoryId);

            // Assert
            Assert.That(expected, Is.EqualTo(actual).Using(new TextMaterialEqualityComparer()), message: "GetByCategory method works incorrectly");
        }

        [Test]
        public async Task TextMaterialRepository_CreateAsync_CreatesTextMaterial()
        {
            // Arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new TextMaterialRepository(context);
            var textMaterial = new TextMaterial
            {
                AuthorId = "1",
                Content = "New text material",
                Title = "New one"
            };

            // Act
            await repository.CreateAsync(textMaterial);
            await context.SaveChangesAsync();

            // Assert
            Assert.That(context.TextMaterials.Count(), Is.EqualTo(4), message: "CreateAsync method works incorrectly");
        }

        [Test]
        public async Task TextMaterialRepository_DeleteById_DeletesTextMaterial()
        {
            // Arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new TextMaterialRepository(context);

            // Act
            await repository.DeleteById(1);
            await context.SaveChangesAsync();

            // Assert
            Assert.That(context.TextMaterials.Count(), Is.EqualTo(2), message: "DeleteById method works incorrectly");
        }

        [Test]
        public async Task TextMaterialRepository_Update_UpdatesTextMaterial()
        {
            // Arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new TextMaterialRepository(context);
            var textMaterial = new TextMaterial()
            {
                Id = 1,
                AuthorId = "1",
                Content = "updatedContent",
                Title = "updatedTItle",
                TextMaterialCategoryId = 1
            };

            // Act
            repository.Update(textMaterial);
            await context.SaveChangesAsync();

            // Assert
            Assert.That(textMaterial, Is.EqualTo(new TextMaterial()
            {
                Id = 1,
                AuthorId = "1",
                Content = "updatedContent",
                Title = "updatedTItle",
                TextMaterialCategoryId = 1
            }).Using(new TextMaterialEqualityComparer()), message: "Update method works incorrectly");
        }

        private static IEnumerable<TextMaterial> ExpectedTextMaterials =
            new[]
            {
                new TextMaterial { Id = 1, AuthorId = "1", ApprovalStatus = ApprovalStatus.Pending, Content = "firstContent", Title = "firstArticle", TextMaterialCategoryId = 1 },
                new TextMaterial { Id = 2, AuthorId = "2", ApprovalStatus = ApprovalStatus.Approved, Content = "secondContent", Title = "secondArticle", TextMaterialCategoryId = 1 },
                new TextMaterial { Id = 3, AuthorId = "2", ApprovalStatus = ApprovalStatus.Approved, Content = "thirdContent", Title = "thirdArticle", TextMaterialCategoryId = 2 }
            };

        private static IEnumerable<TextMaterialCategory> ExpectedTextMaterialCategories =
            new[]
            {
                new TextMaterialCategory { Id = 1, Title = "First one" },
                new TextMaterialCategory { Id = 2, Title = "Second one" }
            };
    }
}
