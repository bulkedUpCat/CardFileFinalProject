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
    public class BanRepository: IBanRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Constructor that accepts the database context
        /// </summary>
        /// <param name="context">Instance of the database context</param>
        public BanRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ban>> GetAsync()
        {
            return await _context.Bans.ToListAsync();
        }

        public async Task<Ban> GetByIdAsync(int id)
        {
            return await _context.Bans.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Ban> GetByUserIdAsync(string userId)
        {
            return await _context.Bans.FirstOrDefaultAsync(b => b.UserId == userId);
        }

        public async Task CreateAsync(Ban entity)
        {
            await _context.Bans.AddAsync(entity);
        }

        public void Update(Ban entity)
        {
            _context.Bans.Update(entity);
        }

        public async Task DeleteById(int id)
        {
            var ban = await _context.Bans.FirstOrDefaultAsync(b => b.Id == id);
            _context.Bans.Remove(ban);
        }

        public async Task DeleteByUserId(string id)
        {
            var ban = await _context.Bans.FirstOrDefaultAsync(b => b.UserId == id);
            _context.Bans.Remove(ban);
        }
    }
}
