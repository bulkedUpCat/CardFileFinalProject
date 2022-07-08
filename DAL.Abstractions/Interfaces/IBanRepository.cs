using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Abstractions.Interfaces
{
    /// <summary>
    /// Interface to be implemented by BanRepository
    /// </summary>
    public interface IBanRepository: IGenericRepository<Ban>
    {
        Task<Ban> GetByIdAsync(int id);
        Task<Ban> GetByUserIdAsync(string userId);
        Task DeleteById(int id);
        Task DeleteByUserId(string id);
    }
}
