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
    public class TextMaterialRepository : ITextMaterialRepository
    {
        private readonly AppDbContext _context;

        public TextMaterialRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TextMaterial>> GetAsync()
        {
            return await _context.TextMaterials.ToListAsync();
        }

        public async Task<IEnumerable<TextMaterial>> GetWithDetailsAsync()
        {
            return await _context.TextMaterials
                .Include(tm => tm.Author)
                .Include(tm => tm.TextMaterialCategory)
                .ToListAsync();
        }

        public async Task<IEnumerable<TextMaterial>> GetWithDetailsAsync(TextMaterialParameters parameters)
        {
            return await _context.TextMaterials
                .Include(tm => tm.Author)
                .Include(tm => tm.TextMaterialCategory)
                .FilterByDatePublished(parameters.StartDate, parameters.EndDate)
                .SearchByTitle(parameters.SearchTitle)
                .SearchByCategory(parameters.SearchCategory)
                .SearchByAuthor(parameters.SearchAuthor)
                .SearchByUserId(parameters.UserId)
                .FilterByApprovalStatus(parameters.ApprovalStatus)
                .Sort(parameters.OrderBy)
                .ToListAsync();
        }

        public async Task<IEnumerable<TextMaterial>> GetByUser(User user)
        {
            return await _context.TextMaterials
                .Include(tm => tm.Author)
                .Where(tm => tm.Author == user)
                .ToListAsync();
        }

        public async Task<TextMaterial> GetByIdAsync(int id)
        {
            return await _context.TextMaterials.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TextMaterial> GetByIdWithDetailsAsync(int id)
        {
            return await _context.TextMaterials
                .Include(tm => tm.Author)
                .Include(tm => tm.TextMaterialCategory)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateAsync(TextMaterial entity)
        {
            await _context.TextMaterials.AddAsync(entity);
        }

        public async Task<IEnumerable<TextMaterial>> GetByCategory(TextMaterialCategory category)
        {
            return await _context.TextMaterials.Where(tm => tm.TextMaterialCategory == category).ToListAsync();
        }

        public void Approve(TextMaterial textMaterial)
        {
            _context.Update(textMaterial);
        }

        public Task DeleteEntity(TextMaterial entity)
        {
            throw new NotImplementedException();
        }

        public void Update(TextMaterial entity)
        {
            _context.Update(entity);
        }
    }
}
