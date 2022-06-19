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
                .Include(tm => tm.UsersWhoSaved)
                .ToListAsync();
        }

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

        public async Task<TextMaterial> GetByIdAsync(int id)
        {
            return await _context.TextMaterials.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TextMaterial> GetByIdWithDetailsAsync(int id)
        {
            return await _context.TextMaterials
                .Include(tm => tm.Author)
                .Include(tm => tm.TextMaterialCategory)
                .Include(tm => tm.UsersWhoSaved)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateAsync(TextMaterial entity)
        {
            await _context.TextMaterials.AddAsync(entity);
        }

        public async Task<IEnumerable<TextMaterial>> GetByCategoryId(int categoryId)
        {
            return await _context.TextMaterials.Where(tm => tm.TextMaterialCategoryId == categoryId).ToListAsync();
        }

        public async Task DeleteById(int id)
        {
            var textMaterial = await _context.TextMaterials.FirstOrDefaultAsync(tm => tm.Id == id);
            _context.TextMaterials.Remove(textMaterial);
        }

        public void Update(TextMaterial entity)
        {
            _context.TextMaterials.Update(entity);
        }
    }
}
