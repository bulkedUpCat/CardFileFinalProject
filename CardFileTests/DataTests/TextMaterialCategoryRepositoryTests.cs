using Core.Models;
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
    public class TextMaterialCategoryRepositoryTests
    {
        [Test]
        public async Task TextMaterialCategoryRepository_GetAsync_ReturnsAllValues()
        {
            // Arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new TextMaterialCategoryRepository(context);
            var expected = ExpectedTextMaterialCategories.ToList();

            // Act
            var actual = await repository.GetAsync();

            // Assert
            Assert.That(actual, Is.EquivalentTo(expected).Using(new TextMaterialCategoryEqualityComparer()), message: "GetAsync method works incorrectly");
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task TextMaterialCategoryRepository_GetByIdAsync_ReturnsSingleValue(int id)
        {
            // Arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new TextMaterialCategoryRepository(context);
            var expected = ExpectedTextMaterialCategories.FirstOrDefault(tmc => tmc.Id == id);

            // Act
            var actual = await repository.GetByIdAsync(id);

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(new TextMaterialCategoryEqualityComparer()), message: "GetByIdAsync method works incorrectly");
        }

        [TestCase("First one")]
        [TestCase("Second one")]
        public async Task TextMaterialCategoryRepository_GetByTitleAsync_ReturnsSingleValue(string title)
        {
            // Arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new TextMaterialCategoryRepository(context);
            var expected = ExpectedTextMaterialCategories.FirstOrDefault(tmc => tmc.Title == title);

            // Act
            var actual = await repository.GetByTitleAsync(title);

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(new TextMaterialCategoryEqualityComparer()), message: "GetByTitleAsync method works incorrectly");
        }

        [Test]
        public async Task TextMaterialCategoryRepository_CreateAsync_CreatesCategory()
        {
            // Arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new TextMaterialCategoryRepository(context);
            var textMaterialCategory = new TextMaterialCategory
            {
                Title = "Third one"
            };

            // Act
            await repository.CreateAsync(textMaterialCategory);
            await context.SaveChangesAsync();

            // Assert
            Assert.That(context.TextMaterialCategory.Count(), Is.EqualTo(3), message: "CreateAsync method works incorrectly");
        }

        [Test]
        public async Task TextMaterialCategoryRepository_Update_UpdatesCategoryInDatabase()
        {
            // Arrange
            var category = ExpectedTextMaterialCategories.First();
            category.Title = "Updated title";
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new TextMaterialCategoryRepository(context);

            // Act
            repository.Update(category);
            await context.SaveChangesAsync();

            // Assert
            Assert.That(category, Is.EqualTo(new TextMaterialCategory
            {
                Id = 1,
                Title = "Updated title"
            }).Using(new TextMaterialCategoryEqualityComparer()), message: "Update method works incorrectly");
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task TextMaterialCategoryRepository_Delete_DeletesCategoryFromDatabase(int id)
        {
            // Arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new TextMaterialCategoryRepository(context);

            // Act
            repository.Delete(id);
            await context.SaveChangesAsync();

            // Assert
            Assert.That(context.TextMaterialCategory.Count(), Is.EqualTo(1), message: "Delete method works incorrectly");
        }

        private static IEnumerable<TextMaterialCategory> ExpectedTextMaterialCategories =
            new[]
            {
                new TextMaterialCategory { Id = 1, Title = "First one" },
                new TextMaterialCategory { Id = 2, Title = "Second one" }
            };
    }
}
