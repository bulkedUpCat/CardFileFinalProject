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
    /// Interface to be implemented by UserRepository
    /// </summary>
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<IEnumerable<User>> GetWithDetailsAsync(UserParameters parameters);
        Task<IEnumerable<TextMaterial>> GetSavedTextMaterialsByUserId(string userId);
        Task<User> GetByIdAsync(string id);
    }
}
