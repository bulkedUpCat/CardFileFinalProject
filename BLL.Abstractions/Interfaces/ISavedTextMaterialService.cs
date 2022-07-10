using Core.DTOs;
using Core.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Abstractions.cs.Interfaces
{
    /// <summary>
    /// Interface to be implemented by classes to work with a list of saved text materialss
    /// </summary>
    public interface ISavedTextMaterialService
    {
        Task<PagedList<TextMaterialDTO>> GetSavedTextMaterialsOfUser(string userId, TextMaterialParameters textMaterialParams);
        Task AddTextMaterialToSaved(string userId, int textMaterialId);
        Task RemoveTextMaterialFromSaved(string userId, int textMaterialId);
    }
}
