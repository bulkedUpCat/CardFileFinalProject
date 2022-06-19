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
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetWithDetailsAsync()
        {
            return await _context.Users
                .Include(u => u.TextMaterials)
                .ToListAsync();
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await _context.Users
                .Include(u => u.TextMaterials)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<TextMaterial>> GetSavedTextMaterialsByUserId(string userId)
        {
            return (await _context.Users.Include(u => u.SavedTextMaterials)
                .FirstOrDefaultAsync(u => u.Id == userId)).SavedTextMaterials;
        }

        public async Task CreateAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
        }

        public void DeleteEntity(User entity)
        {
            _context.Users.Remove(entity);
        }

        public void Update(User entity)
        {
            _context.Users.Update(entity);
        }
    }
}
