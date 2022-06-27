using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Abstractions.cs.Interfaces
{
    /// <summary>
    /// Interface to implemented by classes which perform various operation on comments
    /// </summary>
    public interface ICommentService
    {
        Task<IEnumerable<CommentDTO>> GetCommentsOfTextMaterial(int textMaterialId);
        Task<CommentDTO> CreateComment(CreateCommentDTO createCommentDTO);
        Task<CommentDTO> UpdateComment(UpdateCommentDTO commentDTO);
        Task DeleteComment(int id);
    }
}
