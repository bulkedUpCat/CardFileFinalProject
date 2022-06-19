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
    public class RoleService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<string>> GetRolesAsync()
        {
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return roles;
        }

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
