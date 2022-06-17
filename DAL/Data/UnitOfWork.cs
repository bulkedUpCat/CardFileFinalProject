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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private ITextMaterialRepository _textMaterialRepository;
        private ITextMaterialCategoryRepository _textMaterialCategoryRepository;
        private IUserRepository _userRepository;

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

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
