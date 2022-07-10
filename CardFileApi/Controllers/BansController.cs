using BLL.Services;
using BLL.Validation;
using CardFileApi.Logging;
using Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CardFileApi.Controllers
{
    /// <summary>
    /// Controller that provides endpoints for working with bans
    /// </summary>
    [ApiVersion("1.0")]
    //[Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Route("api/bans")]
    public class BansController : ControllerBase
    {
        private readonly BanService _banService;
        private readonly ILoggerManager _logger;

        public BansController(BanService banService,
            ILoggerManager logger)
        {
            _banService = banService;
            _logger = logger;
        }

        /// <summary>
        /// Returns all existing bans from the database
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllBans()
        {
            var bans = await _banService.GetAllBans();

            return Ok(bans);
        }

        /// <summary>
        /// Returns single ban with given id
        /// </summary>
        /// <param name="id">Id of the ban to return</param>
        [HttpGet("{id}", Name = "GetBanById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var ban = await _banService.GetBanById(id);

            if (ban == null)
            {
                _logger.LogInfo($"Ban with id {id} doesn't exist");
                return NotFound();
            }

            return Ok(ban);
        }

        /// <summary>
        /// Returns ban with given user id
        /// </summary>
        /// <param name="id">Id of the user</param>
        [HttpGet("users/{id}", Name = "GetBanByUserId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByUserId(string id)
        {
            var ban = await _banService.GetBanByUserId(id);

            if (ban == null)
            {
                _logger.LogInfo($"User with id {id} doesn't have a ban");
                return NotFound();
            }

            return Ok(ban);
        }

        /// <summary>
        /// Creates a ban in the database
        /// </summary>
        /// <param name="ban">Model of a new ban to create</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BanUser([FromBody] CreateBanDTO ban)
        {
            /*try
            {*/
                var createdBan = await _banService.BanUser(ban);

                return CreatedAtRoute("GetBanById", new { id = createdBan.Id }, createdBan);
           /* }
            catch (CardFileException e)
            {
                _logger.LogInfo($"Failed to create a ban: {e.Message}");
                return BadRequest(e.Message);
            }*/
        }

        /// <summary>
        /// Updates ban in the database
        /// </summary>
        /// <param name="id">Id of an existing ban</param>
        /// <param name="ban">Model that contains new data of the ban</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateBan(int id, [FromBody] UpdateBanDTO ban)
        {
            try
            {
                var updateddBan = await _banService.UpdateExistingBan(ban);

                return Ok(updateddBan);
            }
            catch (CardFileException e)
            {
                _logger.LogInfo($"Failed to update a ban: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Deletes a ban with given id
        /// </summary>
        /// <param name="id">Id of the ban to delete</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteBanById(int id)
        {
            try
            {
                await _banService.DeleteBanById(id);

                return NoContent();
            }
            catch (CardFileException e)
            {
                _logger.LogInfo($"Failed to delete a ban by its id: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Deletes a bna with given user id
        /// </summary>
        /// <param name="id">Id of the user to unban</param>
        [HttpDelete("users/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteBanByUserId(string id)
        {
            try
            {
                await _banService.DeleteBanByUserId(id);

                return NoContent();
            }
            catch (CardFileException e)
            {
                _logger.LogInfo($"Failed to delete a ban by user id: {e.Message}");
                return BadRequest(e.Message);
            }
        }
    }
}
