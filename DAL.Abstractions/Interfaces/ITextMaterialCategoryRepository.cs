using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Abstractions.Interfaces
{
    /// <summary>
    /// Interface to be implemented by TextMaterialCategoryRepository
    /// </summary>
    public interface ITextMaterialCategoryRepository : IGenericRepository<TextMaterialCategory>
    {
        Task<TextMaterialCategory> GetByTitleAsync(string title);
        Task<TextMaterialCategory> GetByIdAsync(int id);
        void Delete(int id);
    }
}
