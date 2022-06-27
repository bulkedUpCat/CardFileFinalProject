using Core.DTOs;
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
    /// Repository to work with User entities
    /// </summary>
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// Readonly field of the database context to pull and push entities in and out of the database
        /// </summary>
        private readonly AppDbContext _context;

        /// <summary>
        /// Constructor with one argument
        /// </summary>
        /// <param name="context">Instance of database context</param>
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Finds all User entities in the database
        /// </summary>
        /// <returns>List of all User entities from the database</returns>
        public async Task<IEnumerable<User>> GetAsync()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// Finds all User entities in the database along with their relations
        /// </summary>
        /// <returns>List of all User entities from the datbase including their TextMaterials</returns>
        public async Task<IEnumerable<User>> GetWithDetailsAsync()
        {
            return await _context.Users
                .Include(u => u.TextMaterials)
                .ToListAsync();
        }

        /// <summary>
        /// Finds a User entity in the database by its id
        /// </summary>
        /// <param name="id">Id of the User entity to find</param>
        /// <returns>User entity with given id including its TextMaterials, LikedTextMaterials</returns>
        public async Task<User> GetByIdAsync(string id)
        {
            return await _context.Users
                .Include(u => u.TextMaterials)
                .Include(u => u.LikedTextMaterials)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        /// <summary>
        /// Finds all TextMaterial entities which are in the saved of the User entity with given id
        /// </summary>
        /// <param name="userId">Id of the User entity</param>
        /// <returns>List of all TextMaterial entities which are in the saved list of User entity with given id</returns>
        public async Task<IEnumerable<TextMaterial>> GetSavedTextMaterialsByUserId(string userId)
        {
            return (await _context.Users.Include(u => u.SavedTextMaterials)
                .FirstOrDefaultAsync(u => u.Id == userId)).SavedTextMaterials;
        }

        /// <summary>
        /// Adds User entity to the database
        /// </summary>
        /// <param name="entity">User entity to add to the database</param>
        /// <returns>Task representing an asynchronous operation</returns>
        public async Task CreateAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
        }

        /// <summary>
        /// Removes User entity from the database
        /// </summary>
        /// <param name="entity">User entity to remove from the database</param>
        public void DeleteEntity(User entity)
        {
            _context.Users.Remove(entity);
        }

        /// <summary>
        /// Updates User entity in the database
        /// </summary>
        /// <param name="entity">User entity to update in the database</param>
        public void Update(User entity)
        {
            _context.Users.Update(entity);
        }
    }
}
