using BLL.Services;
using BLL.Validation;
using Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CardFileApi.Controllers
{
    [ApiController]
    [Route("api/roles")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly RoleService _roleService;

        public RolesController(RoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleService.GetRolesAsync();

            if (roles == null)
            {
                return NotFound("Found no roles");
            }

            return Ok(roles);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToRole(UserRoleDTO userRole)
        {
            try
            {
                await _roleService.AddUserRoleAsync(userRole.UserId, userRole.RoleName);

                return NoContent();
            }
            catch (CardFileException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveUserFromRole(UserRoleDTO userRole)
        {
            try
            {
                await _roleService.RemoveUserFromRole(userRole.UserId, userRole.RoleName);

                return NoContent();
            }
            catch (CardFileException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
