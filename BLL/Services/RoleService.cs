using BLL.Validation;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    /// <summary>
    /// Service to perform various operations regarding roles such as getting all role names from the database,
    /// adding a specified by id user to a particular role, removing a specified by id user from the particular role
    /// </summary>
    public class RoleService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Constructor which takes two arguments
        /// </summary>
        /// <param name="userManager">Instance of class UserManager to be able to perform operations on users</param>
        /// <param name="roleManager">Instance of class RoleManager to be able to perform operations on roles</param>
        public RoleService(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Returns the names of all existing roles
        /// </summary>
        /// <returns>Names of existing roles</returns>
        public async Task<IEnumerable<string>> GetRolesAsync()
        {
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return roles;
        }

        /// <summary>
        /// Adds user to the specified role
        /// </summary>
        /// <param name="userId">Id of the user to assign a role to</param>
        /// <param name="roleName">Name of the role to assign</param>
        /// <returns>Task if data was valid</returns>
        /// <exception cref="CardFileException"></exception>
        public async Task AddUserRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new CardFileException($"User with id {userId} doesn't exist");
            }

            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                throw new CardFileException($"Role with name {roleName} doesn't exist");
            }

            var result = await _userManager.AddToRoleAsync(user, role.Name);

            if (!result.Succeeded)
            {
                throw new CardFileException($"Failed to add user with name {user.UserName} to role {role.Name}");
            }
        }

        /// <summary>
        /// Removes user from the specified role
        /// </summary>
        /// <param name="userId">Id of the user to remove a role from</param>
        /// <param name="roleName">Name of the role to remove</param>
        /// <returns>Task if data was valid</returns>
        /// <exception cref="CardFileException"></exception>
        public async Task RemoveUserFromRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new CardFileException($"User with id {userId} doesn't exist");
            }

            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                throw new CardFileException($"Role with name {roleName} doesn't exist");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, role.Name);

            if (!result.Succeeded)
            {
                throw new CardFileException($"Failed to remove user with name {user.UserName} from role {role.Name}");
            }
        }
    }
}
