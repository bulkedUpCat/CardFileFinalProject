using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Abstractions.cs.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<string>> GetRolesAsync();
        Task AddUserRoleAsync(string userId, string roleName);
        Task RemoveUserFromRole(string userId, string roleName);
    }
}
