using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Abstractions.Interfaces
{
    public interface IUnitOfWork
    {
        ITextMaterialRepository TextMaterialRepository { get; }
        ITextMaterialCategoryRepository TextMaterialCategoryRepository { get; }
        Task SaveChangesAsync();
    }
}
