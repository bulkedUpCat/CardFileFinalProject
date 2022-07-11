using BLL.Abstractions.cs.Interfaces;
using BLL.Services;
using BLL.Validation;
using CardFileApi.Logging;
using Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading.Tasks;

namespace CardFileApi.Controllers
{
    /// <summary>
    /// Controller that provides endpoints for working with roles
    /// </summary>
    [ApiVersion("1.0")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Route("api/roles")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILoggerManager _logger;

        /// <summary>
        /// Constructor with two parameter
        /// </summary>
        /// <param name="roleService">Instance of class that implements IRoleService interface to work with roles</param>
        /// <param name="logger">Instance of class ILoggerManager to log information</param>
        public RolesController(IRoleService roleService,
            ILoggerManager logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        /// <summary>
        /// Returns all role names that exist in the database
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleService.GetRolesAsync();

            return Ok(roles);
        }

        /// <summary>
        /// Adds specified user to the specified role
        /// </summary>
        /// <param name="userRole">Model that contains data of the user and the role</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddUserToRole(UserRoleDTO userRole)
        {
            await _roleService.AddUserRoleAsync(userRole.UserId, userRole.RoleName);

            return Ok("User added to role");
        }

        /// <summary>
        /// Removes specified user from the specified role
        /// </summary>
        /// <param name="userRole">Model that contains data of the user and the role</param>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveUserFromRole(UserRoleDTO userRole)
        {
            await _roleService.RemoveUserFromRole(userRole.UserId, userRole.RoleName);

            return Ok("User removed from role");
        }
    }
}
