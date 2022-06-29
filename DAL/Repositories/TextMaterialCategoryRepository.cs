using Core.Models;
using DAL.Abstractions.Interfaces;
using DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    /// <summary>
    /// Repository to work with TextMaterialCategory entities
    /// </summary>
    public class TextMaterialCategoryRepository : ITextMaterialCategoryRepository
    {
        /// <summary>
        /// Readonly field of the database context to pull and push entities in and out of the database
        /// </summary>
        private readonly AppDbContext _context;

        /// <summary>
        /// Constructor that accepts the database context
        /// </summary>
        /// <param name="context">Instance of the database context</param>
        public TextMaterialCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Finds all TextMaterialCategory entities in the database
        /// </summary>
        /// <returns>All TextMaterialCategory entities from the database</returns>
        public async Task<IEnumerable<TextMaterialCategory>> GetAsync()
        {
            return await _context.TextMaterialCategory
                .ToListAsync();
        }

        /// <summary>
        /// Finds a single TextMaterialCategory entity in the database by its id
        /// </summary>
        /// <param name="id">Id of TextMaterialCategory entity to find</param>
        /// <returns>Single TextMaterialCategory entity with given id</returns>
        public async Task<TextMaterialCategory> GetByIdAsync(int id)
        {
            return await _context.TextMaterialCategory
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Finds a single TextMaterialCategory entity in the database by its title
        /// </summary>
        /// <param name="title">Title of TextMaterialCategory entity to find</param>
        /// <returns>Single TextMaterialCategory entity with given title</returns>
        public async Task<TextMaterialCategory> GetByTitleAsync(string title)
        {
            return await _context.TextMaterialCategory
                .FirstOrDefaultAsync(x => x.Title == title);
        }

        /// <summary>
        /// Adds TextMaterialCategory entity to the database
        /// </summary>
        /// <param name="entity">TextMaterialCategory entity to add to the database</param>
        /// <returns>Task representing an asynchronous operation</returns>
        public async Task CreateAsync(TextMaterialCategory entity)
        {
            await _context.TextMaterialCategory.AddAsync(entity);
        }

        /// <summary>
        /// Updates TextMaterialCategory entity in the database
        /// </summary>
        /// <param name="entity">TextMaterialCategory entity to update in the database</param>
        public void Update(TextMaterialCategory entity)
        {
            _context.Update(entity);
        }

        /// <summary>
        /// Removes TextMaterialCategory entity from the database by its id
        /// </summary>
        /// <param name="id">Id of TextMaterialCategory entity to remove from the database</param>
        public void Delete(int id)
        {
            var category = _context.TextMaterialCategory.FirstOrDefault(tmc => tmc.Id == id);
            _context.TextMaterialCategory.Remove(category);
        }
    }
}
