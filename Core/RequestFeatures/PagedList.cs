using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RequestFeatures
{
    /// <summary>
    /// Class that converts a list of entities into a paged list, meaning there won't be all entities in it, only the specified number
    /// </summary>
    /// <typeparam name="T">List of entities to be converted into a paged list</typeparam>
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        /// <summary>
        /// Constructor that accepts the source of entities, number of entities, number of the page and the size of the page (number of entities on a single page)
        /// </summary>
        /// <param name="items">List of the entities</param>
        /// <param name="count">Number of the entities</param>
        /// <param name="pageNumber">Number of the page</param>
        /// <param name="pageSize">Size of the page or number of the entities on a single page</param>
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            CurrentPage = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        /// <summary>
        /// Converts a list of entities into a paged list taking into account the number of the page, and the size of the page
        /// </summary>
        /// <param name="source">List of all entities</param>
        /// <param name="pageNumber">Number of the page</param>
        /// <param name="pageSize">Size of the page or number of the entities on a single page</param>
        /// <returns>List converted into a paged list</returns>
        public static PagedList<T> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
