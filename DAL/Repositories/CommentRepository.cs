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
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetCommentsOfTextMaterial(int textMaterialId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Where(c => c.TextMaterialId == textMaterialId)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Comment> GetCommentById(int? id)
        {
            return await _context.Comments
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task CreateAsync(Comment entity)
        {
            await _context.Comments.AddAsync(entity);
        }

        public void Update(Comment entity)
        {
            _context.Comments.Update(entity);
        }

        public async Task Delete(int id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            _context.Comments.Remove(comment);
        }
    }
}
