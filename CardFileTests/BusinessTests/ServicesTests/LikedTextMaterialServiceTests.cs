using BLL.Services;
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
    public class LikedTextMaterialServiceTests
    {
        [TestCase("1")]
        [TestCase("2")]
        public async Task LikedTextMaterialService_GetLikedTextMaterialsByUserId_ReturnsCorrectValues(string userId)
        {
            // Arrange
            var user = GetUserEntities.FirstOrDefault(u => u.Id == userId);
            var expected = GetTextMaterialDTOs;
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(userId))
                .ReturnsAsync(GetUserEntities.FirstOrDefault(u => u.Id == userId));

            var likedTextMaterialService = new LikedTextMaterialService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            var actual = await likedTextMaterialService.GetLikedTextMaterialsByUserId(userId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task LikedTextMaterialService_AddTextMaterialToLiked_AddsTextMaterialToLikedInDatabase()
        {
            // Arrange
            var user = new User { Id = "1" };
            var textMaterial = new TextMaterial { Id = 1 };
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            mockUnitOfWork
                .Setup(x => x.TextMaterialRepository.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(textMaterial);
            mockUnitOfWork
                .Setup(x => x.TextMaterialRepository.Update(It.IsAny<TextMaterial>()));

            var likedTextMaterialService = new LikedTextMaterialService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            await likedTextMaterialService.AddTextMaterialToLiked(user.Id, 1);

            // Assert
            mockUnitOfWork.Verify(x => x.TextMaterialRepository.Update(It.Is<TextMaterial>(tm => tm.Id == textMaterial.Id && tm.UsersWhoLiked.Contains(user))), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task LikedTextMaterialService_RemoveTextMaterialFromSaved_RemovesTextMaterialFromSavedInDatabase()
        {
            // Arrange
            var user = new User { Id = "1" };
            var textMaterial = new TextMaterial { Id = 1, UsersWhoLiked = new List<User> { user } };
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            mockUnitOfWork
                .Setup(x => x.TextMaterialRepository.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(textMaterial);
            mockUnitOfWork
                .Setup(x => x.TextMaterialRepository.Update(It.IsAny<TextMaterial>()));

            var likedTextMaterialService = new LikedTextMaterialService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            await likedTextMaterialService.RemoveTextMaterialFromLiked(user.Id, 1);

            // Assert
            mockUnitOfWork.Verify(x => x.TextMaterialRepository.Update(It.Is<TextMaterial>(tm => tm.Id == textMaterial.Id && !tm.UsersWhoLiked.Contains(user))), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        public List<User> GetUserEntities =>
            new List<User>()
            {
                new User { Id = "1", UserName = "Tommy", Email = "tommy@gmail.com", LikedTextMaterials = GetTextMaterials },
                new User { Id = "2", UserName = "Johnny", Email = "johnny@gmail.com",  LikedTextMaterials = GetTextMaterials }
            };

        public List<TextMaterial> GetTextMaterials =>
            new List<TextMaterial>
            {
                new TextMaterial { Id = 1, UsersWhoLiked = new List<User> { new User(), new User() }, DatePublished = new DateTime(2000,1,1) },
                new TextMaterial { Id = 2, UsersWhoLiked = new List<User> { new User(), new User() }, DatePublished = new DateTime(2003,1,1) }
            };

        public List<TextMaterialDTO> GetTextMaterialDTOs =>
            new List<TextMaterialDTO>
            {
                new TextMaterialDTO { Id = 1, LikesCount = 2, DatePublished = new DateTime(2000,1,1) },
                new TextMaterialDTO { Id = 2, LikesCount = 2, DatePublished = new DateTime(2003,1,1) }
            };
    }
}
