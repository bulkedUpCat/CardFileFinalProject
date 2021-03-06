using BLL.Abstractions.cs.Interfaces;
using BLL.Services;
using BLL.Validation;
using Core.DTOs;
using Core.Models;
using Core.RequestFeatures;
using DAL.Abstractions.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFileTests.BusinessTests.ServicesTests
{
    public class UserServiceTests
    {
        [Test]
        public async Task UserService_GetAll_ReturnsAllUsers()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockStore = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(mockStore.Object, null, null, null, null, null, null, null, null);
            var mockEmailService = new Mock<IEmailService>();
            var expected = GetUserDTOs;
            mockUnitOfWork
                .Setup(x => x.UserRepository.GetWithDetailsAsync(It.IsAny<UserParameters>()))
                .ReturnsAsync(GetUserEntities);
            mockUserManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()));
            mockUserManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()));

            var userService = new UserService(mockUnitOfWork.Object, mockUserManager.Object, UnitTestHelper.CreateMapperProfile(), mockEmailService.Object);

            // Act
            var actual = await userService.GetAll(new UserParameters());

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("1")]
        [TestCase("2")]
        public async Task UserService_GetUserById(string id)
        {
            // Arrange
            var expected = GetUserDTOs.FirstOrDefault(u => u.Id == id);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockStore = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(mockStore.Object, null, null, null, null, null, null, null, null);
            var mockEmailService = new Mock<IEmailService>();
            mockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(id))
                .ReturnsAsync(GetUserEntities.FirstOrDefault(u => u.Id == id));
            mockUserManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()));

            var userService = new UserService(mockUnitOfWork.Object, mockUserManager.Object, UnitTestHelper.CreateMapperProfile(), mockEmailService.Object);

            // Act
            var actual = await userService.GetUserById(id);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("0")]
        [TestCase("-222")]
        public async Task UserService_GetUserById_ThrowsExceptionIfIdIsInvalid(string id)
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockStore = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(mockStore.Object, null, null, null, null, null, null, null, null);
            var mockEmailService = new Mock<IEmailService>();
            mockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(id))
                .ReturnsAsync(GetUserEntities.FirstOrDefault(u => u.Id == id));
            mockUserManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()));

            var userService = new UserService(mockUnitOfWork.Object, mockUserManager.Object, UnitTestHelper.CreateMapperProfile(), mockEmailService.Object);

            // Act
            Func<Task> act = async () => await userService.GetUserById(id);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [TestCase("1",true)]
        [TestCase("2",false)]
        public async Task UserService_ToggleReceiveNotifications_SetsReceiveNotificationsStatus(string userId, bool receiveNotifications)
        {
            // Arrange
            var user = GetUserEntities.FirstOrDefault(u => u.Id == userId);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockStore = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(mockStore.Object, null, null, null, null, null, null, null, null);
            var mockEmailService = new Mock<IEmailService>();
            mockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(userId))
                .ReturnsAsync(GetUserEntities.FirstOrDefault(u => u.Id == userId));
            mockUnitOfWork
                .Setup(x => x.UserRepository.Update(It.IsAny<User>()));

            var userService = new UserService(mockUnitOfWork.Object, mockUserManager.Object, UnitTestHelper.CreateMapperProfile(), mockEmailService.Object);

            // Act
            await userService.ToggleReceiveNotifications(userId, receiveNotifications);

            // Assert
            mockUnitOfWork.Verify(x => x.UserRepository.Update(It.Is<User>(u => u.Id == userId && u.ReceiveNotifications == receiveNotifications)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [TestCase("0", true)]
        [TestCase("-1", true)]
        public async Task UserService_ToggleReceiveNotifications_ThrowsExceptionIfIdInvalid(string userId, bool receiveNotifications)
        {
            // Arrange
            var user = GetUserEntities.FirstOrDefault(u => u.Id == userId);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockStore = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(mockStore.Object, null, null, null, null, null, null, null, null);
            var mockEmailService = new Mock<IEmailService>();
            mockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(userId))
                .ReturnsAsync(GetUserEntities.FirstOrDefault(u => u.Id == userId));
            mockUnitOfWork
                .Setup(x => x.UserRepository.Update(It.IsAny<User>()));

            var userService = new UserService(mockUnitOfWork.Object, mockUserManager.Object, UnitTestHelper.CreateMapperProfile(), mockEmailService.Object);

            // Act
            Func<Task> act = async () => await userService.ToggleReceiveNotifications(userId, receiveNotifications);

            // Assert
            await act.Should().ThrowAsync<CardFileException>();
        }

        public List<User> GetUserEntities =
            new List<User>
            {
                new User { Id = "1", UserName = "Bob Smith", Email = "bobby@gmail.com" },
                new User { Id = "2", UserName = "John Cena", Email = "cena@gmail.com" }
            };

        public List<UserDTO> GetUserDTOs =
            new List<UserDTO>
            {
                new UserDTO { Id = "1", UserName = "Bob Smith", Email = "bobby@gmail.com", Roles = null, TextMaterials = new List<TextMaterialDTO>() },
                new UserDTO { Id = "2", UserName = "John Cena", Email = "cena@gmail.com", Roles = null, TextMaterials = new List<TextMaterialDTO>() }
            };
    }
}
