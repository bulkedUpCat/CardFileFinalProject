using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Abstractions.cs.Interfaces
{
    /// <summary>
    /// Interface to be implemented by classes to work with a list of liked text materials
    /// </summary>
    public interface ILikedTextMaterialService
    {
        Task<IEnumerable<TextMaterialDTO>> GetLikedTextMaterialsByUserId(string userId);
        Task AddTextMaterialToLiked(string userId, int textMaterialId);
        Task RemoveTextMaterialFromLiked(string userId, int textMaterialId);
    }
}
