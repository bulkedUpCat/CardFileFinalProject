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
    public class CommentRepositoryTests
    {
        [Test]
        public async Task CommentRepository_GetAsync_ReturnsAllComments()
        {
            // Arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new CommentRepository(context);
            var expected = ExpectedComments.ToList();

            // Act
            var actual = await repository.GetAsync();

            // Assert
            Assert.That(actual, Is.EquivalentTo(expected).Using(new CommentRepositoryEqualityComparer()), message: "GetAsync method works incorrectly");
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task CommentRepository_GetCommentById_ReturnsSingleValue(int id)
        {
            // Arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new CommentRepository(context);
            var expected = ExpectedComments.FirstOrDefault(c => c.Id == id);

            // Act
            var actual = await repository.GetCommentById(id);

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(new CommentRepositoryEqualityComparer()), message: "GetCommentById method works incorrectly");
        }

        [Test]
        public async Task CommentRepository_CreateAsync_AddsModelToDatabase()
        {
            // Arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new CommentRepository(context);
            var comment = new Comment
            {
                Id = 4,
                Content = "fourth comment",
                UserId = "1",
                TextMaterialId = 1
            };

            // Act
            await repository.CreateAsync(comment);
            await context.SaveChangesAsync();

            // Assert
            Assert.That(context.Comments.Count, Is.EqualTo(4), message: "CreateAsync method works incorrectly");
        }

        [Test]
        public async Task CommetRepository_Delete_RemovesModelFromDatabase()
        {
            // Arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new CommentRepository(context);

            // Act
            await repository.Delete(1);
            await context.SaveChangesAsync();

            // Assert
            Assert.That(context.Comments.Count, Is.EqualTo(2), message: "Delete method works incorreclty");
        }

        [TestCase(1)]
        [TestCase(3)]
        public async Task CommentRepository_GetCommentsOfTextMaterial_ReturnsCommentsOfTextMaterial(int textMaterialId)
        {
            // Arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new CommentRepository(context);
            var expected = ExpectedComments.Where(c => c.TextMaterialId == textMaterialId).ToList();

            // Act
            var actual = await repository.GetCommentsOfTextMaterial(textMaterialId);

            // Assert
            Assert.That(actual, Is.EquivalentTo(expected).Using(new CommentRepositoryEqualityComparer()), message: "GetCommentsOfTextMaterial method works incorrectly");
        }

        [Test]
        public async Task CommentRepository_Update_UpdatesModelInDatabase()
        {
            // Arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new CommentRepository(context);
            var comment = new Comment
            {
                Id = 1,
                Content = "updated first comment",
                UserId = "1",
                TextMaterialId = 1
            };

            // Act
            repository.Update(comment);
            await context.SaveChangesAsync();

            // Assert
            Assert.That(comment, Is.EqualTo(new Comment
            {
                Id = 1,
                Content = "updated first comment",
                UserId = "1",
                TextMaterialId = 1
            }).Using(new CommentRepositoryEqualityComparer()), message: "Update method works incorrectly");
        }

        public static IEnumerable<Comment> ExpectedComments =
            new[]
            {
                new Comment { Id = 1, Content = "first comment", UserId = "1", TextMaterialId = 1 },
                new Comment { Id = 2, Content = "second comment", UserId = "1", TextMaterialId = 1 },
                new Comment { Id = 3, Content = "third comment", UserId = "2", TextMaterialId = 3 }
            };
    }
}
