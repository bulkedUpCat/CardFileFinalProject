using BLL.Services;
using Core.DTOs;
using Core.Models;
using Core.RequestFeatures;
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
    public class SavedTextMaterialServiceTests
    {
        [TestCase("1")]
        [TestCase("2")]
        public async Task SavedTextMaterialService_GetSavedTextMaterialsOfUser_ReturnsCorrectValues(string userId)
        {
            // Arrange
            var expected = new List<TextMaterialDTO> { GetTextMaterialDTOs[0], GetTextMaterialDTOs[1] };
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(userId))
                .ReturnsAsync(GetUserEntities.FirstOrDefault(u => u.Id == userId));
            mockUnitOfWork
                .Setup(x => x.TextMaterialRepository.GetWithDetailsAsync(It.IsAny<TextMaterialParameters>()))
                .ReturnsAsync(GetTextMaterialEntities);

            var savedTextMaterialService = new SavedTextMaterialService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            var actual = await savedTextMaterialService.GetSavedTextMaterialsOfUser(userId, new TextMaterialParameters());

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task SavedTextMaterialService_AddTextMaterialToSaved_AddsTextMaterialToUsersSaved()
        {
            // Arrange
            var user = GetUserEntities.First();
            var textMaterial = GetTextMaterialEntities.FirstOrDefault(tm => tm.Id == 3);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            mockUnitOfWork
                .Setup(x => x.TextMaterialRepository.GetByIdWithDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(textMaterial);
            mockUnitOfWork
                .Setup(x => x.TextMaterialRepository.Update(It.IsAny<TextMaterial>()));

            var savedTextMaterialService = new SavedTextMaterialService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            await savedTextMaterialService.AddTextMaterialToSaved(user.Id, textMaterial.Id);

            // Assert
            mockUnitOfWork.Verify(x => x.TextMaterialRepository.Update(It.Is<TextMaterial>(tm => tm.Id == textMaterial.Id && tm.UsersWhoSaved.Any(u => u.Id == user.Id))), Times.Once());
            mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [Test]
        public async Task SavedTextMaterialService_RemoveTextMaterialFromSaved_RemovesTextMaterialFromUsersSaved()
        {
            // Arrange
            var user = GetUserEntities.First();
            var textMaterial = GetTextMaterialEntities.First();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            mockUnitOfWork
                .Setup(x => x.TextMaterialRepository.GetByIdWithDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(textMaterial);
            mockUnitOfWork
                .Setup(x => x.TextMaterialRepository.Update(It.IsAny<TextMaterial>()));

            var savedTextMaterialService = new SavedTextMaterialService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            await savedTextMaterialService.RemoveTextMaterialFromSaved(user.Id, textMaterial.Id);

            // Assert
            mockUnitOfWork.Verify(x => x.TextMaterialRepository.Update(It.Is<TextMaterial>(tm => tm.Id == textMaterial.Id && !tm.UsersWhoSaved.Any(u => u.Id == user.Id))), Times.Once());
            mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        public List<User> GetUserEntities =>
           new List<User>()
           {
                new User { Id = "1", UserName = "Tommy", Email = "tommy@gmail.com", SavedTextMaterials = new List<TextMaterial>{ new TextMaterial { Id = 1 }, new TextMaterial { Id = 2 } } },
                new User { Id = "2", UserName = "Johnny", Email = "johnny@gmail.com",  SavedTextMaterials = new List<TextMaterial>(){ new TextMaterial { Id = 1 }, new TextMaterial { Id = 2 } } }
           };

        public List<TextMaterial> GetTextMaterialEntities =>
            new List<TextMaterial>
            {
                new TextMaterial { Id = 1, UsersWhoSaved = new List<User>{ GetUserEntities[0] }, DatePublished = new DateTime(2000,1,1) },
                new TextMaterial { Id = 2, UsersWhoSaved = new List<User> { new User(), new User() }, DatePublished = new DateTime(2003,1,1) },
                new TextMaterial { Id = 3, UsersWhoSaved = new List<User>(), DatePublished = new DateTime(2005,1,1) }
            };

        public List<TextMaterialDTO> GetTextMaterialDTOs =>
            new List<TextMaterialDTO>
            {
                new TextMaterialDTO { Id = 1, DatePublished = new DateTime(2000,1,1) },
                new TextMaterialDTO { Id = 2, DatePublished = new DateTime(2003,1,1) },
                new TextMaterialDTO { Id = 3, DatePublished = new DateTime(2005,1,1) }
            };
    }
}
