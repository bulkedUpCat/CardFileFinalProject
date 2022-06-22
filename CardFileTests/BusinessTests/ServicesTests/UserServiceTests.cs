using BLL.Services;
using BLL.Validation;
using Core.DTOs;
using Core.Models;
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
            var expected = GetUserDTOs;
            mockUnitOfWork
                .Setup(x => x.UserRepository.GetWithDetailsAsync())
                .ReturnsAsync(GetUserEntities);
            mockUserManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()));
            mockUserManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()));

            var userService = new UserService(mockUnitOfWork.Object, mockUserManager.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            var actual = await userService.GetAll();

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
            mockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(id))
                .ReturnsAsync(GetUserEntities.FirstOrDefault(u => u.Id == id));
            mockUserManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()));

            var userService = new UserService(mockUnitOfWork.Object, mockUserManager.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            var actual = await userService.GetUserById(id);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("")]
        [TestCase("-222")]
        public async Task UserService_GetUserById_ThrowsExceptionIfIdIsInvalid(string id)
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockStore = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(mockStore.Object, null, null, null, null, null, null, null, null);
            mockUnitOfWork
                .Setup(x => x.UserRepository.GetByIdAsync(id))
                .ReturnsAsync(GetUserEntities.FirstOrDefault(u => u.Id == id));
            mockUserManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()));

            var userService = new UserService(mockUnitOfWork.Object, mockUserManager.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            Func<Task> act = async () => await userService.GetUserById(id);

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
