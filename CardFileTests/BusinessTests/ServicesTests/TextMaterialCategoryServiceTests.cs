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
    public class TextMaterialCategoryServiceTests
    {
        [Test]
        public async Task TextMaterialCategoryService_GetTextMaterialCategoriesAsync_ReturnsAllValues()
        {
            // Arrange
            var expected = GetTextMaterialCategoryDTOs;
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.TextMaterialCategoryRepository.GetAsync())
                .ReturnsAsync(GetTextMaterialCategoryEntities);

            var textMaterialCategoryService = new TextMaterialCategoryService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            var actual = await textMaterialCategoryService.GetTextMaterialCategoriesAsync();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task TextMaterialCategoryService_GetTextMaterialCategoryById_ReturnsSingleValue(int id)
        {
            // Arrange
            var expected = GetTextMaterialCategoryDTOs.FirstOrDefault(tm => tm.Id == id);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.TextMaterialCategoryRepository.GetByIdAsync(id))
                .ReturnsAsync(GetTextMaterialCategoryEntities.FirstOrDefault(tm => tm.Id == id));

            var textMaterialCategoryService = new TextMaterialCategoryService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            var actual = await textMaterialCategoryService.GetTextMaterialCategoryById(id);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task TextMaterialCategoryService_CreateTextMaterialCategoryAsync_AddsTextMaterialCategoryToDatabase()
        {
            // Arrange
            var textMaterialCategory = new CreateTextMaterialCategoryDTO()
            {
                Title = "New category"
            };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.TextMaterialCategoryRepository.GetByTitleAsync(It.IsAny<string>()));
            mockUnitOfWork
                .Setup(x => x.TextMaterialCategoryRepository.CreateAsync(It.IsAny<TextMaterialCategory>()));

            var textMaterialCategoryService = new TextMaterialCategoryService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            await textMaterialCategoryService.CreateTextMaterialCategoryAsync(textMaterialCategory);

            // Assert
            mockUnitOfWork
                .Verify(x => x.TextMaterialCategoryRepository.CreateAsync(It.Is<TextMaterialCategory>(tmc => tmc.Title == textMaterialCategory.Title)),Times.Once);
            mockUnitOfWork
                .Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [TestCase("First one")]
        public async Task TextMaterialCategoryService_CreateTextMaterialCategoryAsync_ThrowsExceptionIfSameTitleExists(string title)
        {
            // Arrange
            var textMaterialCategory = new CreateTextMaterialCategoryDTO()
            {
                Title = title
            };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.TextMaterialCategoryRepository.GetByTitleAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTextMaterialCategoryEntities.FirstOrDefault(tmc => tmc.Title == title));
            mockUnitOfWork
                .Setup(x => x.TextMaterialCategoryRepository.CreateAsync(It.IsAny<TextMaterialCategory>()));

            var textMaterialCategoryService = new TextMaterialCategoryService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            Func<Task> act = async () => await textMaterialCategoryService.CreateTextMaterialCategoryAsync(textMaterialCategory);

            // Assert
            await act.Should().ThrowAsync<CardFileException>();
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task TextMaterialCategoryService_DeleteTextMaterialCategoryAsync_RemovesEntityFromDatabase(int id)
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.TextMaterialCategoryRepository.GetByIdAsync(id))
                .ReturnsAsync(GetTextMaterialCategoryEntities.FirstOrDefault(tmc => tmc.Id == id));
            mockUnitOfWork
                .Setup(x => x.TextMaterialCategoryRepository.Delete(id));

            var textMaterialCategoryService = new TextMaterialCategoryService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            await textMaterialCategoryService.DeleteTextMaterialCategoryAsync(id);

            // Assert
            mockUnitOfWork.Verify(x => x.TextMaterialCategoryRepository.Delete(id), Times.Once());
            mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [TestCase(0)]
        [TestCase(-1)]
        public async Task TextMaterialCategoryService_DeleteTextMaterialCategoryAsync_ThrowsExceptionIfIdInvalid(int id)
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.TextMaterialCategoryRepository.GetByIdAsync(id))
                .ReturnsAsync(GetTextMaterialCategoryEntities.FirstOrDefault(tmc => tmc.Id == id));
            mockUnitOfWork
                .Setup(x => x.TextMaterialCategoryRepository.Delete(id));

            var textMaterialCategoryService = new TextMaterialCategoryService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            Func<Task> act = async () => await textMaterialCategoryService.DeleteTextMaterialCategoryAsync(id);

            // Assert
            await act.Should().ThrowAsync<CardFileException>();
        }

        public List<TextMaterialCategory> GetTextMaterialCategoryEntities =>
            new List<TextMaterialCategory>()
            {
                new TextMaterialCategory { Id = 1, Title = "First one" },
                new TextMaterialCategory { Id = 2, Title = "Second one" },
                new TextMaterialCategory { Id = 3, Title = "Third one" }
            };

        public List<TextMaterialCategoryDTO> GetTextMaterialCategoryDTOs =>
            new List<TextMaterialCategoryDTO>()
            {
                new TextMaterialCategoryDTO { Id = 1, Title = "First one" },
                new TextMaterialCategoryDTO { Id = 2, Title = "Second one" },
                new TextMaterialCategoryDTO { Id = 3, Title = "Third one" }
            };
    }
}
