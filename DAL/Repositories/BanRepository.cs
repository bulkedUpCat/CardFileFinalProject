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
    /// Repository to work with Ban entities
    /// </summary>
    public class BanRepository: IBanRepository
    {
        /// <summary>
        /// Readonly field of the database context to pull and push entities in and out of the database
        /// </summary>
        private readonly AppDbContext _context;

        /// <summary>
        /// Constructor that accepts the database context
        /// </summary>
        /// <param name="context">Instance of the database context</param>
        public BanRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Finds all Ban entities in the database
        /// </summary>
        /// <returns>List of all ban entities from the database</returns>
        public async Task<IEnumerable<Ban>> GetAsync()
        {
            return await _context.Bans.ToListAsync();
        }

        /// <summary>
        /// Finds single Ban entity by its id
        /// </summary>
        /// <param name="id">Id of Ban entity to return</param>
        /// <returns>Single Ban entity with given id</returns>
        public async Task<Ban> GetByIdAsync(int id)
        {
            return await _context.Bans.FirstOrDefaultAsync(b => b.Id == id);
        }

        /// <summary>
        /// Finds single Ban entity by its user's id
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns>Single Ban entity with given user id</returns>
        public async Task<Ban> GetByUserIdAsync(string userId)
        {
            return await _context.Bans.FirstOrDefaultAsync(b => b.UserId == userId);
        }

        /// <summary>
        /// Adds ban entity to the database
        /// </summary>
        /// <param name="entity">Ban entity to add to the database</param>
        /// <returns>Task representing an asynchronous operation</returns>
        public async Task CreateAsync(Ban entity)
        {
            await _context.Bans.AddAsync(entity);
        }

        /// <summary>
        /// Updates Ban entity in the database
        /// </summary>
        /// <param name="entity">Ban entity to update in the database</param>
        public void Update(Ban entity)
        {
            _context.Bans.Update(entity);
        }

        /// <summary>
        /// Removes Ban entity from the database by its id
        /// </summary>
        /// <param name="id">Id of the Ban entity to remove from the database</param>
        /// <returns>Task representing an asycnhronous operation</returns>
        public async Task DeleteById(int id)
        {
            var ban = await _context.Bans.FirstOrDefaultAsync(b => b.Id == id);
            _context.Bans.Remove(ban);
        }

        /// <summary>
        /// Removes Ban entity from the database by user id
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <returns>Task representing an asycnronous operation</returns>
        public async Task DeleteByUserId(string id)
        {
            var ban = await _context.Bans.FirstOrDefaultAsync(b => b.UserId == id);
            _context.Bans.Remove(ban);
        }
    }
}
