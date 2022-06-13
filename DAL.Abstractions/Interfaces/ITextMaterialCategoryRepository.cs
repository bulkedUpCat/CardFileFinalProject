using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Abstractions.Interfaces
{
    public interface ITextMaterialCategoryRepository : IGenericRepository<TextMaterialCategory>
    {
        Task<TextMaterialCategory> GetByTitleAsync(string title);
    }
}
