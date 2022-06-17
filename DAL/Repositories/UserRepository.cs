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

        public async Task<IEnumerable<TextMaterial>> GetSavedTextMaterialsByUserId(string userId)
        {
            return (await _context.Users.Include(u => u.SavedTextMaterials)
                .FirstOrDefaultAsync(u => u.Id == userId)).SavedTextMaterials;
        }

        public Task CreateAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteEntity(User entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
