using BLL.Abstractions.cs.Interfaces;
using BLL.Services;
using BLL.Validation;
using Core.DTOs;
using Core.Models;
using DAL.Abstractions.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
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
    public class AuthServiceTests
    {
        [TestCase("bobby@gmail.com")]
        [TestCase("cena@gmail.com")]
        public async Task AuthService_LogInAsync_ReturnsUser(string email)
        {
            // Arrange
            var user = new UserLoginDTO
            {
                Email = email,
                Password = "pass18"
            };
            var expected = GetUserEntities.FirstOrDefault(u => u.Email == email);
            var mockStore = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(mockStore.Object, null, null, null, null, null, null, null, null);
            var mockSignInManager = new Mock<SignInManager<User>>(mockUserManager.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                null,null,null,null);
            var mockEmailSender = new Mock<IEmailSender>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.BanRepository.GetByUserIdAsync(It.IsAny<string>()));
            mockUserManager
                .Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(GetUserEntities.FirstOrDefault(u => u.Email == email));
            mockSignInManager
                .Setup(x => x.PasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), false, false))
                .ReturnsAsync(SignInResult.Success);

            var authService = new AuthService(mockUserManager.Object, mockSignInManager.Object, mockEmailSender.Object, mockUnitOfWork.Object);

            // Act
            var actual = await authService.LogInAsync(user);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("invalid")]
        [TestCase("")]
        public async Task AuthService_LogInAsync_ThrowsExceptionIfEmailInvalid(string email)
        {
            // Arrange
            var user = new UserLoginDTO
            {
                Email = email,
                Password = "pass18"
            };
            var expected = GetUserEntities.FirstOrDefault(u => u.Email == email);
            var mockStore = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(mockStore.Object, null, null, null, null, null, null, null, null);
            var mockSignInManager = new Mock<SignInManager<User>>(mockUserManager.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                null, null, null, null);
            var mockEmailSender = new Mock<IEmailSender>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUserManager
                .Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(GetUserEntities.FirstOrDefault(u => u.Email == email));
            mockSignInManager
                .Setup(x => x.PasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), false, false))
                .ReturnsAsync(SignInResult.Success);

            var authService = new AuthService(mockUserManager.Object, mockSignInManager.Object, mockEmailSender.Object, mockUnitOfWork.Object);

            // Act
            Func<Task> act = async () => await authService.LogInAsync(user);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedException>();
        }

        [Test]
        public async Task AuthService_LogInAsync_ThrowsExceptionIfPasswordIsIncorrect()
        {
            // Arrange
            var user = new UserLoginDTO
            {
                Email = "bobby@gmail.com",
                Password = "pass18"
            };
            var expected = GetUserEntities[0];
            var mockStore = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(mockStore.Object, null, null, null, null, null, null, null, null);
            var mockSignInManager = new Mock<SignInManager<User>>(mockUserManager.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                null, null, null, null);
            var mockEmailSender = new Mock<IEmailSender>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUserManager
                .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetUserEntities[0]);
            mockSignInManager
                .Setup(x => x.PasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), false, false))
                .ReturnsAsync(SignInResult.Failed);

            var authService = new AuthService(mockUserManager.Object, mockSignInManager.Object, mockEmailSender.Object, mockUnitOfWork.Object);

            // Act
            Func<Task> act = async () => await authService.LogInAsync(user);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedException>();
        }
/*
        [Test]
        public async Task AuthService_SignUpAsync_ReturnsNewUser()
        {
            // Arrange
            var user = new UserRegisterDTO
            {
                Email = "newUser@gmail.com",
                Name = "NewUser",
                Password = "pass18",
                ConfirmPassword = "pass18"
            };
            var mockStore = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(mockStore.Object, null, null, null, null, null, null, null, null);
            var mockSignInManager = new Mock<SignInManager<User>>(mockUserManager.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                null, null, null, null);
            var mockEmailSender = new Mock<IEmailSender>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUserManager
                .Setup(x => x.FindByEmailAsync(It.IsAny<string>()));
            mockUserManager
                .Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var authService = new AuthService(mockUserManager.Object, mockSignInManager.Object, mockEmailSender.Object, mockUnitOfWork.Object);

            // Act
            await authService.SignUpAsync(user);

            // Assert
            mockUserManager.Verify(x => x.CreateAsync(It.Is<User>(u => u.Email == user.Email && u.UserName == user.Name), It.Is<string>(s => s == user.Password)), Times.Once());
        }*/

        [TestCase("bobby@gmail.com")]
        [TestCase("cena@gmail.com")]
        public async Task AuthService_SignUpAsync_ThrowsExceptionIfUserWithSameEmailAlreadyExists(string email)
        {
            // Arrange
            var user = new UserRegisterDTO
            {
                Email = email,
                Name = "NewUser",
                Password = "pass18",
                ConfirmPassword = "pass18"
            };
            var mockStore = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(mockStore.Object, null, null, null, null, null, null, null, null);
            var mockSignInManager = new Mock<SignInManager<User>>(mockUserManager.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                null, null, null, null);
            var mockEmailSender = new Mock<IEmailSender>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUserManager
                .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetUserEntities.FirstOrDefault(u => u.Email == email));
            mockUserManager
                .Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var authService = new AuthService(mockUserManager.Object, mockSignInManager.Object, mockEmailSender.Object, mockUnitOfWork.Object);

            // Act
            Func<Task> act = async () => await authService.SignUpAsync(user);

            // Assert
            await act.Should().ThrowAsync<CardFileException>();
        }

        [Test]
        public async Task AuthService_ConfirmEmail_SetsEmailConfirmedToTrue()
        {
            // Arrange
            var model = new ConfirmEmailDTO
            {
                Email = "newEmail@gmail.com",
                Token = "valid token"
            };

            var mockEmailSender = new Mock<IEmailSender>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockStore = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(mockStore.Object, null, null, null, null, null, null, null, null);
            var mockSignInManager = new Mock<SignInManager<User>>(mockUserManager.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                null, null, null, null);
            mockUserManager
                .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetUserEntities.First());
            mockUserManager
                .Setup(x => x.ConfirmEmailAsync(It.IsAny<User>(), model.Token))
                .ReturnsAsync(IdentityResult.Success);

            var authService = new AuthService(mockUserManager.Object, mockSignInManager.Object, mockEmailSender.Object, mockUnitOfWork.Object);

            // Act
            await authService.ConfirmEmail(model);

            // Assert
            mockUserManager.Verify(x => x.FindByEmailAsync(It.Is<string>(e => e == model.Email)), Times.Once);
            mockUserManager.Verify(x => x.ConfirmEmailAsync(It.Is<User>(u => u == GetUserEntities.First()), It.Is<string>(s => s == model.Token)), Times.Once);
        }

        [TestCase("invalidGMAIL")]
        [TestCase("anonymous@gmail.com")]
        public async Task AuthService_ConfirmEmail_ThrowsExceptionIfEmailInvalid(string email)
        {
            // Arrange
            var model = new ConfirmEmailDTO
            {
                Email = email,
                Token = "valid token"
            };

            var mockEmailSender = new Mock<IEmailSender>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockStore = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(mockStore.Object, null, null, null, null, null, null, null, null);
            var mockSignInManager = new Mock<SignInManager<User>>(mockUserManager.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                null, null, null, null);
            mockUserManager
                .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetUserEntities.FirstOrDefault(u => u.Email == email));
            mockUserManager
                .Setup(x => x.ConfirmEmailAsync(It.IsAny<User>(), model.Token))
                .ReturnsAsync(IdentityResult.Success);

            var authService = new AuthService(mockUserManager.Object, mockSignInManager.Object, mockEmailSender.Object, mockUnitOfWork.Object);

            // Act
            Func<Task> act = async () => await authService.ConfirmEmail(model);

            // Assert
            await act.Should().ThrowAsync<CardFileException>();
        }


        public List<User> GetUserEntities =
           new List<User>
           {
                new User { Id = "1", UserName = "Bob Smith", Email = "bobby@gmail.com" },
                new User { Id = "2", UserName = "John Cena", Email = "cena@gmail.com" }
           };
    }
}
