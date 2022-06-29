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
    /// <summary>
    /// Extension class for TextMaterialRepository to implement sorting, filtering and searching
    /// </summary>
    public static class TextMaterialRepositoryExtensions
    {
        /// <summary>
        /// Extension method to filter text materials by publishment date
        /// </summary>
        /// <param name="textMaterials">Text materials to filter</param>
        /// <param name="startDate">Start publishment date</param>
        /// <param name="endDate">End publishment date</param>
        /// <returns>Filtered text materials</returns>
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

        /// <summary>
        /// Extension method to search text materials by title
        /// </summary>
        /// <param name="textMaterials">Text materials to be searched</param>
        /// <param name="searchTitle">Search title to search text materials by</param>
        /// <returns>Text materials with titles that contain search string</returns>
        public static IQueryable<TextMaterial> SearchByTitle(this IQueryable<TextMaterial> textMaterials, string searchTitle)
        {
            if (string.IsNullOrEmpty(searchTitle))
            {
                return textMaterials;
            }

            var lowerCaseTerm = searchTitle.Trim().ToLower();
            return textMaterials.Where(tm => tm.Title.ToLower().Contains(lowerCaseTerm));
        }

        /// <summary>
        /// Extension method to search text materials by category title
        /// </summary>
        /// <param name="textMaterials">Text materials to be searched</param>
        /// <param name="searchCategory">Category title to search text materials by</param>
        /// <returns>Text materials with category titles that contain search string</returns>
        public static IQueryable<TextMaterial> SearchByCategory(this IQueryable<TextMaterial> textMaterials, string searchCategory)
        {
            if (string.IsNullOrEmpty(searchCategory))
            {
                return textMaterials;
            }

            var lowerCaseTerm = searchCategory.Trim().ToLower();
            return textMaterials.Where(tm => tm.TextMaterialCategory.Title.ToLower().Contains(lowerCaseTerm));
        }

        /// <summary>
        /// Extension method to search text materials by author name
        /// </summary>
        /// <param name="textMaterials">Text materials to be searched</param>
        /// <param name="searchAuthor">Author name to search text materials by</param>
        /// <returns>Text materials with author names that contain search string</returns>
        public static IQueryable<TextMaterial> SearchByAuthor(this IQueryable<TextMaterial> textMaterials, string searchAuthor)
        {
            if (string.IsNullOrEmpty(searchAuthor))
            {
                return textMaterials;
            }

            var lowerCaseTerm = searchAuthor.Trim().ToLower();
            return textMaterials.Where(tm => tm.Author.UserName.ToLower().Contains(lowerCaseTerm));
        }

        /// <summary>
        /// Extension method to filter text materails by approval status
        /// </summary>
        /// <param name="textMaterials">Text materials to filter</param>
        /// <param name="approvalStatus">Array of approval status to filter by</param>
        /// <returns>Filtered by approval status text materials</returns>
        public static IQueryable<TextMaterial> FilterByApprovalStatus(this IQueryable<TextMaterial> textMaterials, List<int> approvalStatus)
        {
            if (approvalStatus == null)
            {
                return textMaterials;
            }

            return textMaterials.Where(tm => approvalStatus.Contains((int)tm.ApprovalStatus));
        }

        /// <summary>
        /// Extension method to sort text materials
        /// </summary>
        /// <param name="textMaterials">Text materials to sort</param>
        /// <param name="orderByQueryString">Orderby string to sort by</param>
        /// <returns>Sorted text materials</returns>
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

                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction}, ");
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
