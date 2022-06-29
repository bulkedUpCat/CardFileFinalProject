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
    /// Repository to work with Comment entities
    /// </summary>
    public class CommentRepository : ICommentRepository
    {
        /// <summary>
        /// Readonly field of the database context to pull and push entities in and out of the database
        /// </summary>
        private readonly AppDbContext _context;

        /// <summary>
        /// Constructor that accepts the database context
        /// </summary>
        /// <param name="context">Instance of the database context</param>
        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Finds all Comment entities in the database
        /// </summary>
        /// <returns>List of all Comment entities from the database</returns>
        public async Task<IEnumerable<Comment>> GetAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        /// <summary>
        /// Finds all Comment entities in the database by given TextMaterialId
        /// </summary>
        /// <param name="textMaterialId">Id of TextMaterial</param>
        /// <returns>All Commemnt entities from the database by given TexMaterial id</returns>
        public async Task<IEnumerable<Comment>> GetCommentsOfTextMaterial(int textMaterialId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Where(c => c.TextMaterialId == textMaterialId)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Finds a single Comment entity by its id
        /// </summary>
        /// <param name="id">Id of Comment entity to find</param>
        /// <returns>Single Comment entity with given id</returns>
        public async Task<Comment> GetCommentById(int? id)
        {
            return await _context.Comments
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        /// <summary>
        /// Adds Comemnt entity to the database
        /// </summary>
        /// <param name="entity">Comment entity to add to the database</param>
        /// <returns>Task representing an asynchronous operation</returns>
        public async Task CreateAsync(Comment entity)
        {
            await _context.Comments.AddAsync(entity);
        }

        /// <summary>
        /// Updates Comment entity in the database
        /// </summary>
        /// <param name="entity">Comment entity to update in the database</param>
        public void Update(Comment entity)
        {
            _context.Comments.Update(entity);
        }

        /// <summary>
        /// Removes Comment entity from the database by its id
        /// </summary>
        /// <param name="id">Id of Comment entity to remove from the database</param>
        /// <returns>Task representing an asynchronous operation</returns>
        public async Task Delete(int id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            _context.Comments.Remove(comment);
        }
    }
}
