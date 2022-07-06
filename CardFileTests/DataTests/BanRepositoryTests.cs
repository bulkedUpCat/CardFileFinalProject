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
    public class BanRepositoryTests
    {
        [Test]
        public async Task BanRepository_GetAsync_ReturnsAllBans()
        {
            // Arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new BanRepository(context);
            var expected = ExpectedBans.ToList();

            // Act
            var actual = await repository.GetAsync();

            // Asert
            Assert.That(actual, Is.EquivalentTo(expected).Using(new BanEqualityComparer()), message: "GetAsync method works incorrectly");
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task BanRepository_GetByIdAsync_ReturnsSingleBan(int id)
        {
            // Arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new BanRepository(context);
            var expected = ExpectedBans.FirstOrDefault(b => b.Id == id);

            // Act
            var actual = await repository.GetByIdAsync(id);

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(new BanEqualityComparer()), message: "GetByIdAsync method works incorrectly");
        }

        [TestCase("1")]
        [TestCase("2")]
        public async Task BanRepository_GetByUserIdAsync_ReturnsSingleBan(string userId)
        {
            // Arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new BanRepository(context);
            var expected = ExpectedBans.FirstOrDefault(b => b.UserId == userId);

            // Act
            var actual = await repository.GetByUserIdAsync(userId);

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(new BanEqualityComparer()), message: "GetByUserIdAsync method works incorrectly");
        }

        [Test]
        public async Task BanRepository_CreateAsync_AddsModelToDatabase()
        {
            // Arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new BanRepository(context);
            var ban = new Ban
            {
                Id = 4,
                Reason = "Fourth test ban",
                UserId = "4",
                Expires = new DateTime(2022, 7, 4)
            };

            // Act
            await repository.CreateAsync(ban);
            await context.SaveChangesAsync();

            // Assert
            Assert.That(context.Bans.Count, Is.EqualTo(4), message: "CreateAsync method works incorrectly");
        }

        [Test]
        public async Task BanRepository_Update_UpdatesModelInDatabase()
        {
            // Arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new BanRepository(context);
            var ban = new Ban
            {
                Id = 1,
                Reason = "First updated ban",
                UserId = "1",
                Expires = new DateTime(2001, 1, 6)
            };

            // Act
            repository.Update(ban);
            await context.SaveChangesAsync();

            // Assert
            Assert.That(ban, Is.EqualTo(new Ban
            {
                Id = 1,
                Reason = "First updated ban",
                UserId = "1",
                Expires = new DateTime(2001, 1, 6)
            }).Using(new BanEqualityComparer()), message: "Update method works incorrectly");
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task BanRepository_DeleteById_RemovesModelFromDatabase(int id)
        {
            // Arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new BanRepository(context);

            // Act
            await repository.DeleteById(id);
            await context.SaveChangesAsync();

            // Assert
            Assert.That(context.Bans.Count, Is.EqualTo(2), message: "DeleteById method works incorrectly");
        }

        [TestCase("1")]
        [TestCase("2")]
        public async Task BanRepository_DeleteByUserId_RemovesModelFromDatabase(string userId)
        {
            // Arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var repository = new BanRepository(context);

            // Act
            await repository.DeleteByUserId(userId);
            await context.SaveChangesAsync();

            // Assert
            Assert.That(context.Bans.Count, Is.EqualTo(2), message: "DeleteByUserId method works incorrectly");
        }

        public static IEnumerable<Ban> ExpectedBans =
            new[]
            {
                new Ban { Id = 1, Reason = "First test ban", UserId = "1", Expires = new DateTime(2001, 1, 1) },
                new Ban { Id = 2, Reason = "Second test ban", UserId = "2", Expires = new DateTime(2005, 2, 3) },
                new Ban { Id = 3, Reason = "Third test ban", UserId = "3", Expires = new DateTime(2022, 7, 5) }
            };
    }
}
