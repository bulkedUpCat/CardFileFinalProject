using BLL.Abstractions.cs.Interfaces;
using Core.DTOs;
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
        private readonly IBanService _banService;

        /// <summary>
        /// Constructor which accepts a service for working with bans
        /// </summary>
        /// <param name="banService">Instance of class that implements IBanService interface</param>
        public BansController(IBanService banService)
        {
            _banService = banService;
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

            return Ok(ban);
        }

        /// <summary>
        /// Returns ban with given user id
        /// </summary>
        /// <param name="id">Id of the user</param>
        [HttpGet("users/{id}", Name = "GetBanByUserId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByUserId(string id)
        {
            var ban = await _banService.GetBanByUserId(id);

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
            var createdBan = await _banService.BanUser(ban);

            return CreatedAtRoute("GetBanById", new { id = createdBan.Id }, createdBan);
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
            var updateddBan = await _banService.UpdateExistingBan(ban);

            return Ok(updateddBan);
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
            await _banService.DeleteBanById(id);

            return NoContent();
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
            await _banService.DeleteBanByUserId(id);

            return NoContent();
        }
    }
}
