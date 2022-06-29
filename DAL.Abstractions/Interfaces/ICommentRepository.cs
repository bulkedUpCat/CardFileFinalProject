using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Abstractions.Interfaces
{
    /// <summary>
    /// Interface to be implemented by CommentRepository
    /// </summary>
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetCommentsOfTextMaterial(int textMaterialId);
        Task<Comment> GetCommentById(int? id);
        Task Delete(int id);
    }
}
