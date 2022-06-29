using Core.Models;
using Core.RequestFeatures;
using DAL.Abstractions.Interfaces;
using DAL.Contexts;
using DAL.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    /// <summary>
    /// Repository to work with TextMaterial entities
    /// </summary>
    public class TextMaterialRepository : ITextMaterialRepository
    {
        /// <summary>
        /// Readonly field of the database context to pull and push entities in and out of the database
        /// </summary>
        private readonly AppDbContext _context;

        /// <summary>
        /// Constructror that accepts the database context
        /// </summary>
        /// <param name="context">Instance of database context</param>
        public TextMaterialRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Finds all TextMaterial entities in the databse
        /// </summary>
        /// <returns>List of all TextMaterial entities from the database</returns>
        public async Task<IEnumerable<TextMaterial>> GetAsync()
        {
            return await _context.TextMaterials.ToListAsync();
        }

        /// <summary>
        /// Finds all TextMaterial entities in the database alongs with their relations
        /// </summary>
        /// <returns>List of all TextMaterial entities including relations such as Author, TextMaterialCategory, UsersWhoSaved</returns>
        public async Task<IEnumerable<TextMaterial>> GetWithDetailsAsync()
        {
            return await _context.TextMaterials
                .Include(tm => tm.Author)
                .Include(tm => tm.TextMaterialCategory)
                .Include(tm => tm.UsersWhoSaved)
                .ToListAsync();
        }

        /// <summary>
        /// Finds all TextMaterial entities that satisfy given parameters
        /// </summary>
        /// <param name="parameters">Parameters to sort, filter found TextMaterial entities</param>
        /// <returns>List of sorted, filtered TextMaterial entities that satisfy given parameters</returns>
        public async Task<IEnumerable<TextMaterial>> GetWithDetailsAsync(TextMaterialParameters parameters)
        {
            return await _context.TextMaterials
                .Include(tm => tm.Author)
                .Include(tm => tm.TextMaterialCategory)
                .Include(tm => tm.UsersWhoSaved)
                .FilterByDatePublished(parameters.StartDate, parameters.EndDate)
                .SearchByTitle(parameters.SearchTitle)
                .SearchByCategory(parameters.SearchCategory)
                .SearchByAuthor(parameters.SearchAuthor)
                .FilterByApprovalStatus(parameters.ApprovalStatus)
                .Sort(parameters.OrderBy)
                .ToListAsync();
        }

        /// <summary>
        /// Finds all TextMaterial entities with a given author id that satisfy given parameters
        /// </summary>
        /// <param name="userId">Id of the author of text material</param>
        /// <param name="parameters">Parameters to sort, filter found TextMaterial entities</param>
        /// <returns>List of all TextMaterial entities which are of given author and satisfy given parameters</returns>
        public async Task<IEnumerable<TextMaterial>> GetByUserId(string userId, TextMaterialParameters parameters)
        {
            return await _context.TextMaterials
                .Include(tm => tm.Author)
                .Include(tm => tm.TextMaterialCategory)
                .Include(tm => tm.UsersWhoSaved)
                .FilterByDatePublished(parameters.StartDate, parameters.EndDate)
                .SearchByTitle(parameters.SearchTitle)
                .SearchByCategory(parameters.SearchCategory)
                .FilterByApprovalStatus(parameters.ApprovalStatus)
                .Sort(parameters.OrderBy)
                .Where(tm => tm.AuthorId == userId)
                .ToListAsync();
        }

        /// <summary>
        /// Finds a single TextMaterial entity by its id
        /// </summary>
        /// <param name="id">Id of the TextMaterial entity to find</param>
        /// <returns>A single TextMaterial entity with given id</returns>
        public async Task<TextMaterial> GetByIdAsync(int id)
        {
            return await _context.TextMaterials.FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Finds a single TextMaterial entity by its id along with all of its relations
        /// </summary>
        /// <param name="id">Id of text material to find</param>
        /// <returns>A single TextMaterial entity with given id including its Author, TextMaterialCategory, UsersWhoSaved, UsersWhoLiked</returns>
        public async Task<TextMaterial> GetByIdWithDetailsAsync(int id)
        {
            return await _context.TextMaterials
                .Include(tm => tm.Author)
                .Include(tm => tm.TextMaterialCategory)
                .Include(tm => tm.UsersWhoSaved)
                .Include(tm => tm.UsersWhoLiked)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Adds a TextMaterial entity to the database
        /// </summary>
        /// <param name="entity">TextMaterial entity to add to the database</param>
        /// <returns>Task representing an asynchronous operation</returns>
        public async Task CreateAsync(TextMaterial entity)
        {
            await _context.TextMaterials.AddAsync(entity);
        }

        /// <summary>
        /// Finds all TextMaterial entities by TextMaterialCategory id
        /// </summary>
        /// <param name="categoryId">Id of TextMaterialCategory</param>
        /// <returns>List of all TextMaterial entities which are of given TextMaterialCategory</returns>
        public async Task<IEnumerable<TextMaterial>> GetByCategoryId(int categoryId)
        {
            return await _context.TextMaterials.Where(tm => tm.TextMaterialCategoryId == categoryId).ToListAsync();
        }

        /// <summary>
        /// Removes TextMaterial entity from the database by its id
        /// </summary>
        /// <param name="id">Id of TextMaterial entity to remove from the database</param>
        /// <returns>Task representing an asynchronous operation</returns>
        public async Task DeleteById(int id)
        {
            var textMaterial = await _context.TextMaterials.FirstOrDefaultAsync(tm => tm.Id == id);
            _context.TextMaterials.Remove(textMaterial);
        }

        /// <summary>
        /// Updates TextMaterial entity in the database
        /// </summary>
        /// <param name="entity">TextMatial entity to update in the database</param>
        public void Update(TextMaterial entity)
        {
            _context.TextMaterials.Update(entity);
        }
    }
}
