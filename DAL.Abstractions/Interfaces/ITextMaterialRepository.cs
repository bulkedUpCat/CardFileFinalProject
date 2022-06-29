using Core.Models;
using Core.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Abstractions.Interfaces
{
    /// <summary>
    /// Interface to be implemented by TextMaterialRepository
    /// </summary>
    public interface ITextMaterialRepository : IGenericRepository<TextMaterial>
    {
        Task<IEnumerable<TextMaterial>> GetByCategoryId(int categoryId);
        Task<TextMaterial> GetByIdAsync(int id);
        Task<IEnumerable<TextMaterial>> GetWithDetailsAsync();
        Task<IEnumerable<TextMaterial>> GetWithDetailsAsync(TextMaterialParameters parameters);
        Task<IEnumerable<TextMaterial>> GetByUserId(string userId, TextMaterialParameters parameters);
        Task<TextMaterial> GetByIdWithDetailsAsync(int id);
        Task DeleteById(int id);
    }
}
