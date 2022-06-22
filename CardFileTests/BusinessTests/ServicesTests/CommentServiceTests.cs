using BLL.Services;
using BLL.Validation;
using Core.DTOs;
using Core.Models;
using DAL.Abstractions.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFileTests.BusinessTests.ServicesTests
{
    public class CommentServiceTests
    {
        [TestCase(1)]
        [TestCase(2)]
        public async Task CommentService_GetCommentOfTextMaterial_ReturnsCorrectValues(int id)
        {
            // Arrange
            var expected = GetCommentDTOs.Where(c => c.TextMaterialId == id).ToList();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.TextMaterialRepository.GetByIdAsync(id))
                .ReturnsAsync(GetTextMaterialEntities.FirstOrDefault(tm => tm.Id == id));
            mockUnitOfWork
                .Setup(x => x.CommentRepository.GetCommentsOfTextMaterial(id))
                .ReturnsAsync(GetCommentEntities.Where(c => c.TextMaterialId == id));

            var commentService = new CommentService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            var actual = await commentService.GetCommentsOfTextMaterial(id);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task CommentService_CreateComment_AddsModelToDatabase()
        {
            // Arrange
            var comment = new CreateCommentDTO()
            {
                UserId = "1",
                TextMaterialId = 2,
                Content = "new comment"
            };
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(comment.UserId))
                .ReturnsAsync(GetUserEntities[0]);
            mockUnitOfWork
                .Setup(x => x.TextMaterialRepository.GetByIdAsync(comment.TextMaterialId))
                .ReturnsAsync(GetTextMaterialEntities[1]);
            mockUnitOfWork
                .Setup(x => x.CommentRepository.GetCommentById(It.IsAny<int>()));
            mockUnitOfWork
                .Setup(x => x.CommentRepository.CreateAsync(It.IsAny<Comment>()));

            var commentService = new CommentService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            await commentService.CreateComment(comment);

            // Assert
            mockUnitOfWork.Verify(x => x.CommentRepository.CreateAsync(It.Is<Comment>(c => c.UserId == comment.UserId && c.TextMaterialId == comment.TextMaterialId && c.Content == comment.Content)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveChangesAsync());
        }

        [TestCase("")]
        [TestCase("  ")]
        [TestCase(null)]
        public async Task CommentService_CreateComment_ThrowsExceptionIfContentIsEmpty(string content)
        {
            // Arrange
            var comment = new CreateCommentDTO()
            {
                UserId = "1",
                TextMaterialId = 2,
                Content = content
            };
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(comment.UserId))
                .ReturnsAsync(GetUserEntities[0]);
            mockUnitOfWork
                .Setup(x => x.TextMaterialRepository.GetByIdAsync(comment.TextMaterialId))
                .ReturnsAsync(GetTextMaterialEntities[1]);
            mockUnitOfWork
                .Setup(x => x.CommentRepository.GetCommentById(It.IsAny<int>()));
            mockUnitOfWork
                .Setup(x => x.CommentRepository.CreateAsync(It.IsAny<Comment>()));

            var commentService = new CommentService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            Func<Task> act = async () => await commentService.CreateComment(comment);

            // Assert
            await act.Should().ThrowAsync<CardFileException>();
        }

        [TestCase("0")]
        [TestCase("-2")]
        public async Task CommentService_CreateComment_ThrowsExceptionIfUserIdInvalid(string userId)
        {
            // Arrange
            var comment = new CreateCommentDTO()
            {
                UserId = userId,
                TextMaterialId = 2,
                Content = "new one"
            };
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(comment.UserId))
                .ReturnsAsync(GetUserEntities.FirstOrDefault(u => u.Id == comment.UserId));
            mockUnitOfWork
                .Setup(x => x.TextMaterialRepository.GetByIdAsync(comment.TextMaterialId))
                .ReturnsAsync(GetTextMaterialEntities.FirstOrDefault(u => u.Id == comment.TextMaterialId));
            mockUnitOfWork
                .Setup(x => x.CommentRepository.GetCommentById(It.IsAny<int>()));
            mockUnitOfWork
                .Setup(x => x.CommentRepository.CreateAsync(It.IsAny<Comment>()));

            var commentService = new CommentService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            Func<Task> act = async () => await commentService.CreateComment(comment);

            // Assert
            await act.Should().ThrowAsync<CardFileException>();
        }

        [TestCase(0)]
        public async Task CommentService_CreateComment_ThrowsExceptionIfTextMaterialIdInvalid(int textMaterialId)
        {
            // Arrange
            var comment = new CreateCommentDTO()
            {
                UserId = "1",
                TextMaterialId = textMaterialId,
                Content = "new one"
            };
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(comment.UserId))
                .ReturnsAsync(GetUserEntities.FirstOrDefault(u => u.Id == comment.UserId));
            mockUnitOfWork
                .Setup(x => x.TextMaterialRepository.GetByIdAsync(comment.TextMaterialId))
                .ReturnsAsync(GetTextMaterialEntities.FirstOrDefault(u => u.Id == comment.TextMaterialId));
            mockUnitOfWork
                .Setup(x => x.CommentRepository.GetCommentById(It.IsAny<int>()));
            mockUnitOfWork
                .Setup(x => x.CommentRepository.CreateAsync(It.IsAny<Comment>()));

            var commentService = new CommentService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            Func<Task> act = async () => await commentService.CreateComment(comment);

            // Assert
            await act.Should().ThrowAsync<CardFileException>();
        }

        [Test]
        public async Task CommentService_UpdateComment_UpdatesModelInDatabase()
        {
            // Arrange
            var comment = new UpdateCommentDTO()
            {
                Id = 1,
                Content = "updated content"
            };
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.CommentRepository.GetCommentById(comment.Id))
                .ReturnsAsync(GetCommentEntities.FirstOrDefault(c => c.Id == comment.Id));
            mockUnitOfWork
                .Setup(x => x.CommentRepository.Update(It.IsAny<Comment>()));

            var commentService = new CommentService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            await commentService.UpdateComment(comment);

            mockUnitOfWork.Verify(x => x.CommentRepository.Update(It.Is<Comment>(c => c.Id == comment.Id && c.Content == comment.Content)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [TestCase(0)]
        [TestCase(-110)]
        public async Task CommentService_UpdateComment_ThrowsExceptionIfCommentIdInvalid(int id)
        {
            // Arrange
            var comment = new UpdateCommentDTO()
            {
                Id = id,
                Content = "updated content"
            };
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.CommentRepository.GetCommentById(comment.Id))
                .ReturnsAsync(GetCommentEntities.FirstOrDefault(c => c.Id == comment.Id));
            mockUnitOfWork
                .Setup(x => x.CommentRepository.Update(It.IsAny<Comment>()));

            var commentService = new CommentService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            Func<Task> act = async () => await commentService.UpdateComment(comment);

            // Assert
            await act.Should().ThrowAsync<CardFileException>();
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task CommentService_DeleteComment_RemovesModelFromDatabase(int id)
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.CommentRepository.GetCommentById(id))
                .ReturnsAsync(GetCommentEntities.FirstOrDefault(c => c.Id == id));
            mockUnitOfWork
                .Setup(x => x.CommentRepository.Delete(id));

            var commentService = new CommentService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            await commentService.DeleteComment(id);

            // Assert
            mockUnitOfWork.Verify(x => x.CommentRepository.Delete(id), Times.Once());
            mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public async Task CommentService_DeleteComment_ThrowsExceptionIfIdInvalid(int id)
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.CommentRepository.GetCommentById(id))
                .ReturnsAsync(GetCommentEntities.FirstOrDefault(c => c.Id == id));
            mockUnitOfWork
                .Setup(x => x.CommentRepository.Delete(id));

            var commentService = new CommentService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            Func<Task> act = async () => await commentService.DeleteComment(id);

            // Assert
            await act.Should().ThrowAsync<CardFileException>();
        }

        public List<User> GetUserEntities =>
            new List<User>()
            {
                new User { Id = "1", UserName = "Tommy", Email = "tommy@gmail.com" },
                new User { Id = "2", UserName = "Johnny", Email = "johnny@gmail.com" }
            };

        public List<TextMaterial> GetTextMaterialEntities =>
            new List<TextMaterial>
            {
                new TextMaterial { Id = 1, Author = GetUserEntities[0], AuthorId = "1", ApprovalStatus = ApprovalStatus.Pending, Content = "firstContent", Title = "firstArticle", TextMaterialCategoryId = 1, DatePublished = new DateTime(2000,11,23) },
                new TextMaterial { Id = 2, Author = GetUserEntities[1], AuthorId = "2", ApprovalStatus = ApprovalStatus.Approved, Content = "secondContent", Title = "secondArticle", TextMaterialCategoryId = 1, DatePublished = new DateTime(2010,8,17) },
            };

        public List<Comment> GetCommentEntities =>
            new List<Comment>()
            {
                new Comment { Id = 1, Content = "first comment", TextMaterialId = 1, TextMaterial = GetTextMaterialEntities[0], User = GetUserEntities[0], CreatedAt = new DateTime(2000,1,1) },
                new Comment { Id = 2, Content = "second comment", TextMaterialId = 2, TextMaterial = GetTextMaterialEntities[1], User = GetUserEntities[1], CreatedAt = new DateTime(2010,2,13) },
                new Comment { Id = 3, Content = "third comment", TextMaterialId = 1, TextMaterial = GetTextMaterialEntities[0], User = GetUserEntities[1], CreatedAt = new DateTime(2013, 2, 23) },
            };

        public List<CommentDTO> GetCommentDTOs =>
            new List<CommentDTO>()
            {
                new CommentDTO { Id = 1, UserName = "Tommy", Content = "first comment", TextMaterialId = 1, UserId = "1", CreatedAt = new DateTime(2000,1,1) },
                new CommentDTO { Id = 2, UserName = "Johnny", Content = "second comment", TextMaterialId = 2, UserId = "2", CreatedAt = new DateTime(2010,2,13) },
                new CommentDTO { Id = 3, UserName = "Johnny", Content = "third comment", TextMaterialId = 1, UserId = "2", CreatedAt = new DateTime(2013, 2, 23) },
            };
    }
}
