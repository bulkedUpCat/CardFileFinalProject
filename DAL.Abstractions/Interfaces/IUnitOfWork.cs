using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Abstractions.Interfaces
{
    /// <summary>
    /// Interface to be implemented by UnitOfWork
    /// </summary>
    public interface IUnitOfWork
    {
        ITextMaterialRepository TextMaterialRepository { get; }
        ITextMaterialCategoryRepository TextMaterialCategoryRepository { get; }
        IUserRepository UserRepository { get; }
        ICommentRepository CommentRepository { get; }
        Task SaveChangesAsync();
    }
}
