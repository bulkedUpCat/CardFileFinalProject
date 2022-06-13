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
    public class TextMaterialCategoryRepository : ITextMaterialCategoryRepository
    {
        private readonly AppDbContext _context;

        public TextMaterialCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TextMaterialCategory>> GetAsync()
        {
            return await _context.TextMaterialCategory.ToListAsync();
        }

        public async Task<TextMaterialCategory> GetByIdAsync(int id)
        {
            return await _context.TextMaterialCategory.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TextMaterialCategory> GetByTitleAsync(string title)
        {
            return await _context.TextMaterialCategory.FirstOrDefaultAsync(x => x.Title == title);
        }

        public async Task CreateAsync(TextMaterialCategory entity)
        {
            await _context.TextMaterialCategory.AddAsync(entity);
        }

        public Task DeleteEntity(TextMaterialCategory entity)
        {
            throw new NotImplementedException();
        }

        public void Update(TextMaterialCategory entity)
        {
            throw new NotImplementedException();
        }
    }
}
