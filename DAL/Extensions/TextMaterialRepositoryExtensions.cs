using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Linq;

namespace DAL.Extensions
{
    public static class TextMaterialRepositoryExtensions
    {
        public static IQueryable<TextMaterial> FilterByDatePublished(this IQueryable<TextMaterial> textMaterials, string startDate, string endDate)
        {
            if (string.IsNullOrWhiteSpace(startDate) &&
                string.IsNullOrWhiteSpace(endDate))
            {
                return textMaterials;
            }

            if (string.IsNullOrWhiteSpace(startDate))
            {
                return textMaterials.Where(tm => DateTime.Compare(tm.DatePublished.Date, DateTime.Parse(endDate)) <= 0);
            }
            else if (string.IsNullOrWhiteSpace(endDate))
            {
                return textMaterials.Where(tm => DateTime.Compare(tm.DatePublished.Date, DateTime.Parse(startDate)) >= 0);
            }

            var fromDate = DateTime.Parse(startDate);
            var toDate = DateTime.Parse(endDate);

            return textMaterials.Where(tm =>
                DateTime.Compare(tm.DatePublished.Date, fromDate) >= 0 &&
                DateTime.Compare(tm.DatePublished.Date, toDate) <= 0);
        }

        public static IQueryable<TextMaterial> SearchByTitle(this IQueryable<TextMaterial> textMaterials, string searchTitle)
        {
            if (string.IsNullOrEmpty(searchTitle))
            {
                return textMaterials;
            }

            var lowerCaseTerm = searchTitle.Trim().ToLower();
            return textMaterials.Where(tm => tm.Title.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<TextMaterial> SearchByCategory(this IQueryable<TextMaterial> textMaterials, string searchCategory)
        {
            if (string.IsNullOrEmpty(searchCategory))
            {
                return textMaterials;
            }

            var lowerCaseTerm = searchCategory.Trim().ToLower();
            return textMaterials.Where(tm => tm.TextMaterialCategory.Title.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<TextMaterial> SearchByAuthor(this IQueryable<TextMaterial> textMaterials, string searchAuthor)
        {
            if (string.IsNullOrEmpty(searchAuthor))
            {
                return textMaterials;
            }

            var lowerCaseTerm = searchAuthor.Trim().ToLower();
            return textMaterials.Where(tm => tm.Author.UserName.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<TextMaterial> SearchByUserId(this IQueryable<TextMaterial> textMaterials, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return textMaterials;
            }

            var lowerCaseTerm = userId.Trim().ToLower();
            return textMaterials.Where(tm => tm.AuthorId == lowerCaseTerm);
        }

        public static IQueryable<TextMaterial> FilterByApprovalStatus(this IQueryable<TextMaterial> textMaterials, List<int> approvalStatus)
        {
            if (approvalStatus == null)
            {
                return textMaterials;
            }

            return textMaterials.Where(tm => approvalStatus.Contains((int)tm.ApprovalStatus));
        }

        public static IQueryable<TextMaterial> Sort(this IQueryable<TextMaterial> textMaterials, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return textMaterials;
            }

            var orderParams = orderByQueryString.Trim().Split(",");
            var propertyInfos = typeof(TextMaterial).GetProperties(BindingFlags.Public |
                BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach(var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                {
                    continue;
                }

                var propertyFromQueryName = param.Split(" ")[0];

                var objectProperty = propertyInfos.FirstOrDefault(pi =>
                    pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty == null)
                {
                    continue;
                }

                var direction = param.EndsWith(" desc") ? "descending" : "ascending";

                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction}");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            if (string.IsNullOrWhiteSpace(orderQuery))
            {
                return textMaterials.OrderBy(tm => tm.DatePublished);
            }

            return textMaterials.OrderBy(orderQuery);
        }
    }
}
