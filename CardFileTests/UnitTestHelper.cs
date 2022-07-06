using AutoMapper;
using BLL.Profiles;
using Core.Models;
using DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFileTests
{
    internal static class UnitTestHelper
    {
        public static DbContextOptions<AppDbContext> GetUnitTestDbOptions()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            using (var context = new AppDbContext(options))
            {
                SeedData(context);
            }

            return options;
        }

        public static IMapper CreateMapperProfile()
        {
            var textMaterialProfile = new TextMaterialProfile();
            var textMaterialCategoryProfile = new TextMaterialCategoryProfile();
            var userProfile = new UserProfile();
            var commentProfile = new CommentProfile();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(textMaterialProfile);
                cfg.AddProfile(textMaterialCategoryProfile);
                cfg.AddProfile(userProfile);
                cfg.AddProfile(commentProfile);
            });

            return new Mapper(configuration);
        }

        public static void SeedData(AppDbContext context)
        {
            context.TextMaterials.AddRange(
                new TextMaterial { Id = 1, AuthorId = "1", ApprovalStatus = ApprovalStatus.Pending, Content = "firstContent", Title = "firstArticle", TextMaterialCategoryId = 1, DatePublished = new DateTime(2000,3,12) },
                new TextMaterial { Id = 2, AuthorId = "2", ApprovalStatus = ApprovalStatus.Approved, Content = "secondContent", Title = "secondArticle", TextMaterialCategoryId = 1, DatePublished = new DateTime(2003, 4,23) },
                new TextMaterial { Id = 3, AuthorId = "2", ApprovalStatus = ApprovalStatus.Approved, Content = "thirdContent", Title = "thirdArticle", TextMaterialCategoryId = 2, DatePublished = new DateTime(2004,1,1) });

            context.TextMaterialCategory.AddRange(
                new TextMaterialCategory { Id = 1, Title = "First one" },
                new TextMaterialCategory { Id = 2, Title = "Second one" });

            context.Users.AddRange(
                new User { Id = "1", UserName = "Tommy", Email = "tommy@gmail.com" },
                new User { Id = "2", UserName = "Johnny", Email = "johnny@gmail.com" },
                new User { Id = "3", UserName = "Bobby", Email = "bobby@gmail.com" });

            context.Comments.AddRange(
                new Comment { Id = 1, Content = "first comment", UserId = "1", TextMaterialId = 1, CreatedAt = new DateTime(2007, 11, 12) },
                new Comment { Id = 2, Content = "second comment", UserId = "1", TextMaterialId = 1, CreatedAt = new DateTime(2001, 1, 23) },
                new Comment { Id = 3, Content = "third comment", UserId = "2", TextMaterialId = 3, CreatedAt = new DateTime(2000,12,1) });

            context.Bans.AddRange(
                new Ban { Id = 1, Reason = "First test ban", UserId = "1", Expires = new DateTime(2001, 1, 1) },
                new Ban { Id = 2, Reason = "Second test ban", UserId = "2", Expires = new DateTime(2005, 2, 3) },
                new Ban { Id = 3, Reason = "Third test ban", UserId = "3", Expires = new DateTime(2022, 7, 5) });

            context.SaveChanges();
        }
    }
}
