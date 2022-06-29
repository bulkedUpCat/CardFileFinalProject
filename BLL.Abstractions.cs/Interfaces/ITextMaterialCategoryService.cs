using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Abstractions.cs.Interfaces
{
    /// <summary>
    /// Interface to be implemented by classes to work with text material categories
    /// </summary>
    public interface ITextMaterialCategoryService
    {
        Task<IEnumerable<TextMaterialCategoryDTO>> GetTextMaterialCategoriesAsync();
        Task<TextMaterialCategoryDTO> GetTextMaterialCategoryById(int id);
        Task<TextMaterialCategoryDTO> CreateTextMaterialCategoryAsync(CreateTextMaterialCategoryDTO categoryDTO);
        Task DeleteTextMaterialCategoryAsync(int id);
    }
}
