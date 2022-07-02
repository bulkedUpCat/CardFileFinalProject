using BLL.Services;
using BLL.Validation;
using CardFileApi.Logging;
using Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CardFileApi.Controllers
{
    //[Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Route("api/bans")]
    public class BansController: ControllerBase
    {
        private readonly BanService _banService;
        private readonly ILoggerManager _logger;

        public BansController(BanService banService,
            ILoggerManager logger)
        {
            _banService = banService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllBans()
        {
            var bans = await _banService.GetAllBans();

            if (bans == null)
            {
                return NotFound();
            }

            return Ok(bans);
        }

        [HttpGet("{id}", Name = "GetBanById")]
        public async Task<IActionResult> GetById(int id)
        {
            var ban = await _banService.GetBanById(id);

            if (ban == null)
            {
                return NotFound();
            }

            return Ok(ban);
        }

        [HttpGet("users/{id}", Name = "GetBanByUserId")]
        public async Task<IActionResult> GetByUserId(string id)
        {
            var ban = await _banService.GetBanByUserId(id);

            if (ban == null)
            {
                return NotFound();
            }

            return Ok(ban);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BanUser([FromBody] CreateBanDTO ban)
        {
            try
            {
                var createdBan = await _banService.BanUser(ban);

                return CreatedAtRoute("GetBanById", new { id = createdBan.Id }, createdBan);
            }
            catch (CardFileException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBanById(int id)
        {
            try
            {
                await _banService.DeleteBanById(id);

                return NoContent();
            }
            catch (CardFileException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteBanByUserId(string id)
        {
            try
            {
                await _banService.DeleteBanByUserId(id);

                return NoContent();
            }
            catch (CardFileException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
