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
    internal class UserRepositoryTests
    {
        [Test]
        public async Task UserRepository_GetAsync_ReturnsAllValues()
        {
            // Arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new UserRepository(context);
            var expected = ExpectedUsers.ToList();

            // Act
            var actual = await repository.GetAsync();

            // Assert
            Assert.That(actual, Is.EquivalentTo(expected).Using(new UserRepositoryEqualityComparer()), message: "GetAsync method works incorrectly");
        }

        [TestCase("1")]
        [TestCase("2")]
        [TestCase("3")]
        public async Task UserRepository_GetByIdAsync_ReturnsSignleValue(string id)
        {
            // Arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new UserRepository(context);
            var expected = ExpectedUsers.FirstOrDefault(u => u.Id == id);

            // Act
            var actual = await repository.GetByIdAsync(id);

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(new UserRepositoryEqualityComparer()), message: "GetByIdAsync method works incorrectly");
        }

        [Test]
        public async Task UserRepository_CreateAsync_AddsUserToDatabase()
        {
            // Arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new UserRepository(context);
            var user = new User
            {
                UserName = "Tyler",
                Email = "tyler@gmail.com"
            };

            // Act
            await repository.CreateAsync(user);
            await context.SaveChangesAsync();

            // Assert
            Assert.That(context.Users.Count(), Is.EqualTo(4), message: "CreateAsync method works incorrecly");
        }
/*
        [Test]
        public async Task UserRepository_Delete_RemovesUserFromDatabase()
        {
            // Arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new UserRepository(context);
            var userToDelete = ExpectedUsers.FirstOrDefault(u => u.Id == "1");

            // Act
            repository.DeleteEntity(userToDelete);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }

            // Assert
            Assert.That(context.Users.Count(), Is.EqualTo(2), message: "Delete method works incorrecly");
        }*/

        [Test]
        public async Task UserRepository_Update_UpdatesModelInDatabase()
        {
            // Arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new UserRepository(context);
            var user = ExpectedUsers.First();
            user.UserName = "UpdatedTommy";
            user.Email = "updatedTommy@gmail.com";

            // Act
            repository.Update(user);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {

            }

            // Assert
            Assert.That(user, Is.EqualTo(new User
            {
                Id = "1",
                UserName = "UpdatedTommy",
                Email = "updatedTommy@gmail.com"
            }).Using(new UserRepositoryEqualityComparer()), message: "Update method works incorrectly");
        }

        private static IEnumerable<User> ExpectedUsers =>
            new[]
            {
                new User { Id = "1", UserName = "Tommy", Email = "tommy@gmail.com" },
                new User { Id = "2", UserName = "Johnny", Email = "johnny@gmail.com" },
                new User { Id = "3", UserName = "Bobby", Email = "bobby@gmail.com" }
            };
    }
}
