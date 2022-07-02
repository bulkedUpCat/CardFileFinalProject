using DAL.Abstractions.Interfaces;
using DAL.Contexts;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    /// <summary>
    /// Class that incapsulates working with repositories
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private ITextMaterialRepository _textMaterialRepository;
        private ITextMaterialCategoryRepository _textMaterialCategoryRepository;
        private IUserRepository _userRepository;
        private ICommentRepository _commentRepository;
        private IBanRepository _banRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public ITextMaterialRepository TextMaterialRepository
        {
            get
            {
                if (_textMaterialRepository == null)
                {
                    _textMaterialRepository = new TextMaterialRepository(_context);
                }

                return _textMaterialRepository;
            }
        }

        public ITextMaterialCategoryRepository TextMaterialCategoryRepository
        {
            get
            {
                if (_textMaterialCategoryRepository == null)
                {
                    _textMaterialCategoryRepository = new TextMaterialCategoryRepository(_context);
                }

                return _textMaterialCategoryRepository;
            }
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_context);
                }

                return _userRepository;
            }
        }

        public ICommentRepository CommentRepository
        {
            get
            {
                if (_commentRepository == null)
                {
                    _commentRepository = new CommentRepository(_context);
                }

                return _commentRepository;
            }
        }

        public IBanRepository BanRepository
        {
            get
            {
                if (_banRepository == null)
                {
                    _banRepository = new BanRepository(_context);
                }

                return _banRepository;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
