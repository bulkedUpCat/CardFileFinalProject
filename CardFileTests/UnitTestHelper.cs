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
            var configuration = new MapperConfiguration(cfg =>
            {

            });

            return new Mapper(configuration);
        }

        public static void SeedData(AppDbContext context)
        {
            context.TextMaterials.AddRange(
                new TextMaterial { Id = 1, AuthorId = "1", Content = "firstContent", Title = "firstArticle", TextMaterialCategoryId = 1 },
                new TextMaterial { Id = 2, AuthorId = "2", Content = "secondContent", Title = "secondArticle", TextMaterialCategoryId = 1 },
                new TextMaterial { Id = 3, AuthorId = "2", Content = "thirdContent", Title = "thirdArticle", TextMaterialCategoryId = 2 });

            context.SaveChanges();
        }
    }
}
