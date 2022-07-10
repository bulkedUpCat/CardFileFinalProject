using BLL.Abstractions.cs.Interfaces;
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
    public class BanServiceTests
    {
        [Test]
        public async Task BanService_GetAllBans_ReturnsAllBansFromDatabase()
        {
            // Arrange
            var expected = GetBanModels;
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockEmailService = new Mock<IEmailService>();
            mockUnitOfWork
                .Setup(x => x.BanRepository.GetAsync())
                .ReturnsAsync(GetBanEntities);
            var banService = new BanService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile(), mockEmailService.Object);

            // Act
            var actual = await banService.GetAllBans();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task BanService_GetBanById_ReturnsSingleBan(int id)
        {
            // Arrange
            var expected = GetBanModels.FirstOrDefault(b => b.Id == id);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockEmailService = new Mock<IEmailService>();
            mockUnitOfWork
                .Setup(x => x.BanRepository.GetByIdAsync(id))
                .ReturnsAsync(GetBanEntities.FirstOrDefault(b => b.Id == id));
            var banService = new BanService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile(), mockEmailService.Object);

            // Act
            var actual = await banService.GetBanById(id);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("1")]
        [TestCase("2")]
        public async Task BanService_GetBanByUserId_ReturnsSingleBan(string userId)
        {
            // Arrange
            var expected = GetBanModels.FirstOrDefault(b => b.UserId == userId);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockEmailService = new Mock<IEmailService>();
            mockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(userId))
                .ReturnsAsync(new User());
            mockUnitOfWork
                .Setup(x => x.BanRepository.GetByUserIdAsync(userId))
                .ReturnsAsync(GetBanEntities.FirstOrDefault(b => b.UserId == userId));
            var banService = new BanService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile(), mockEmailService.Object);

            // Act
            var actual = await banService.GetBanByUserId(userId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task BanService_BanUser_AddsModelToDatabase()
        {
            // Arrange
            var ban = new CreateBanDTO
            {
                UserId = "4",
                Days = 10,
                Reason = "no reason"
            };
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockEmailService = new Mock<IEmailService>();
            mockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            mockUnitOfWork
                .Setup(x => x.BanRepository.GetByUserIdAsync(It.IsAny<string>()));
            mockUnitOfWork
                .Setup(x => x.BanRepository.CreateAsync(It.IsAny<Ban>()));
            mockUnitOfWork
                .Setup(x => x.SaveChangesAsync());

            var banService = new BanService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile(), mockEmailService.Object);

            // Act
            await banService.BanUser(ban);

            // Assert
            mockUnitOfWork.Verify(x => x.BanRepository.CreateAsync(It.Is<Ban>(b => b.Reason == ban.Reason && b.UserId == ban.UserId)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [TestCase("0")]
        [TestCase("-2")]
        public async Task BanService_BanUser_ThrowsExceptionIfUserIdInvalid(string userId)
        {
            // Arrange
            var ban = new CreateBanDTO
            {
                UserId = userId,
                Days = 10,
                Reason = "no reason"
            };
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockEmailService = new Mock<IEmailService>();
            mockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<string>()));
            mockUnitOfWork
                .Setup(x => x.BanRepository.GetByUserIdAsync(It.IsAny<string>()));
            mockUnitOfWork
                .Setup(x => x.BanRepository.CreateAsync(It.IsAny<Ban>()));
            mockUnitOfWork
                .Setup(x => x.SaveChangesAsync());

            var banService = new BanService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile(), mockEmailService.Object);

            // Act
            Func<Task> act = async() => await banService.BanUser(ban);

            // Assert
            await act.Should().ThrowAsync<CardFileException>();
        }

        public List<Ban> GetBanEntities =
            new List<Ban>
            {
                new Ban { Id = 1, Reason = "First test ban", UserId = "1", Expires = new DateTime(2001, 1, 1) },
                new Ban { Id = 2, Reason = "Second test ban", UserId = "2", Expires = new DateTime(2005, 2, 3) },
                new Ban { Id = 3, Reason = "Third test ban", UserId = "3", Expires = new DateTime(2022, 7, 5) }
            };

        public List<BanDTO> GetBanModels =
            new List<BanDTO>
            {
                new BanDTO { Id = 1, Reason = "First test ban", UserId = "1", Expires = new DateTime(2001, 1, 1) },
                new BanDTO { Id = 2, Reason = "Second test ban", UserId = "2", Expires = new DateTime(2005, 2, 3) },
                new BanDTO { Id = 3, Reason = "Third test ban", UserId = "3", Expires = new DateTime(2022, 7, 5) }
            };
    }
}
