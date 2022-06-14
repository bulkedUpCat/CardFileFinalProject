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
    public class TextMaterialRepositoryTests
    {
        [Test]
        public async Task TextMaterialRepository_GetAsync_ReturnsAllValues()
        {
            // Arrange
            var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new TextMaterialRepository(context);
            var expected = ExpectedTextMaterials.ToList();

            // Act
            var actual = await repository.GetAsync();

            // Assert
            Assert.That(actual, Is.EquivalentTo(expected).Using(new TextMaterialEqualityComparer()), message: "GetAsync works incorrectly");
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task TextMaterialRepository_GetByIdAsync_ReturnsSingleValue(int id)
        {
            // Arrange
            var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new TextMaterialRepository(context);
            var expected = ExpectedTextMaterials.FirstOrDefault(tm => tm.Id == id);

            // Act
            var textMaterial = await repository.GetByIdAsync(id);

            // Assert
            Assert.That(expected, Is.EqualTo(textMaterial).Using(new TextMaterialEqualityComparer()), message: "GetByIdAsync works incorrectly");
        }

        private static IEnumerable<TextMaterial> ExpectedTextMaterials;
            /*new[]
            {
                //new TextMaterial { Id = 1, AuthorId = "1", Content = "firstContent", Title = "firstArticle", TextMaterialCategoryId = 1 },
                //new TextMaterial { Id = 2, AuthorId = "2", Content = "secondContent", Title = "secondArticle", TextMaterialCategoryId = 1 },
                //new TextMaterial { Id = 3, AuthorId = "2", Content = "thirdContent", Title = "thirdArticle", TextMaterialCategoryId = 2 }
            };*/
    }
}
