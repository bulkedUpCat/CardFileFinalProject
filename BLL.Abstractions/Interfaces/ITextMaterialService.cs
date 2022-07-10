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
    /// Interface to be implemented by class which perform various operations on text materials
    /// </summary>
    public interface ITextMaterialService
    {
        Task<PagedList<TextMaterialDTO>> GetTextMaterials(TextMaterialParameters parameters);
        Task<PagedList<TextMaterialDTO>> GetTextMaterialsOfUser(string id, TextMaterialParameters textMaterialParams);
        Task<TextMaterialDTO> GetTextMaterialById(int id);
        Task<TextMaterialDTO> CreateTextMaterial(CreateTextMaterialDTO textMaterialDTO);
        Task UpdateTextMaterial(UpdateTextMaterialDTO textMaterialDTO);
        Task DeleteTextMaterial(int id);
        Task ApproveTextMaterial(int textMaterialId);
        Task RejectTextMaterial(int textMaterialId, string? rejectMessage = null);
        Task SendTextMaterialAsPdf(string userId, int textMaterialId, EmailParameters emailParams);
    }
}
