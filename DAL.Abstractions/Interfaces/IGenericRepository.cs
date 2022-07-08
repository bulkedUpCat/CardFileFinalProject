using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Abstractions.Interfaces
{
    /// <summary>
    /// Generic repository to be implemented by other repositories
    /// </summary>
    /// <typeparam name="TEntity">Instance of model class</typeparam>
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAsync();
        Task CreateAsync(TEntity entity);
        void Update(TEntity entity);
    }
}
