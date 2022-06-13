using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CardFileApi.Controllers
{
    [ApiController]
    [Route("api/roles")]
    //[Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly RoleService _roleService;

        public RolesController(RoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task Do()
        {
            await _roleService.AssignRoleAsync();
        }
    }
}
