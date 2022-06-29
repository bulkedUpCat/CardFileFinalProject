using Core.DTOs;
using Core.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Abstractions.cs.Interfaces
{
    public interface ISavedTextMaterialService
    {
        Task<PagedList<TextMaterialDTO>> GetSavedTextMaterialsOfUser(string userId, TextMaterialParameters textMaterialParams);
        Task AddTextMaterialToSaved(string userId, int textMaterialId);
        Task RemoveTextMaterialFromSaved(string userId, int textMaterialId);
    }
}
