using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Abstractions.cs.Interfaces
{
    public interface ILikedTextMaterialService
    {
        Task<IEnumerable<TextMaterialDTO>> GetLikedTextMaterialsByUserId(string userId);
        Task AddTextMaterialToLiked(string userId, int textMaterialId);
        Task RemoveTextMaterialFromLiked(string userId, int textMaterialId);
    }
}
