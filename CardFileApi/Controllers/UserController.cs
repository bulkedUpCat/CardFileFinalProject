using BLL.Abstractions.cs.Interfaces;
using BLL.Services;
using BLL.Validation;
using Core.DTOs;
using Core.Models;
using Core.RequestFeatures;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace CardFileApi.Controllers
{
    /// <summary>
    /// Controller that provides endpoints for working with users
    /// </summary>
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ITextMaterialService _textMaterialService;
        private readonly SavedTextMaterialsService _savedTextMaterialService;
        private readonly LikedTextMaterialService _likedTextMaterialService;

        /// <summary>
        /// Constructor that takes four arguments
        /// </summary>
        /// <param name="userService">instance of class that implements IUserService interface to work with users</param>
        /// <param name="textMaterialService">Instance of class that implemetns ITextMaterialService to work with text materials</param>
        /// <param name="savedTextMaterialService">Instance of class that implements ISavedTextMaterialService to work with saving text materials</param>
        /// <param name="likedTextMaterialService">Instance of class that implements ILikedTextMaterialService to work with liking text materials</param>
        public UserController(UserService userService,
            ITextMaterialService textMaterialService,
            SavedTextMaterialsService savedTextMaterialService,
            LikedTextMaterialService likedTextMaterialService)
        {
            _userService = userService;
            _textMaterialService = textMaterialService;
            _savedTextMaterialService = savedTextMaterialService;
            _likedTextMaterialService = likedTextMaterialService;
        }

        /// <summary>
        /// Returns all users from the database by given parameters
        /// </summary>
        /// <param name="userParameters">Parameters that will be taken into account when returning users from the database</param>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<UserDTO>>> Get([FromQuery]UserParameters userParameters)
        {
            var users = await _userService.GetAll(userParameters);

            if (users == null)
            {
                return NotFound("No users were found");
            }

            var metadata = new
            {
                users.TotalCount,
                users.PageSize,
                users.CurrentPage,
                users.TotalPages,
                users.HasNext,
                users.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");

            return Ok(users);
        }

        /// <summary>
        /// Returns the user from the database by its id
        /// </summary>
        /// <param name="id">Id of the user to return</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDTO>> GetById(string id)
        {
            try
            {
                var user = await _userService.GetUserById(id);

                return Ok(user);
            }
            catch (CardFileException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Returns text materials of the user by given author id and given parameters
        /// </summary>
        /// <param name="id">Id of the author of the text materials</param>
        /// <param name="textMaterialParams">Parameters that will be taken into account when returning the text materials</param>
        [HttpGet("{id}/textMaterials", Name = "GetTextMaterialsByUserId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TextMaterialDTO>>> Get(string id,[FromQuery]TextMaterialParameters textMaterialParams)
        {
            try
            {
                var textMaterials = await _textMaterialService.GetTextMaterialsOfUser(id,textMaterialParams);

                var metadata = new
                {
                    textMaterials.TotalCount,
                    textMaterials.PageSize,
                    textMaterials.CurrentPage,
                    textMaterials.TotalPages,
                    textMaterials.HasNext,
                    textMaterials.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");

                return Ok(textMaterials);
            }
            catch (CardFileException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Returns saved text materials of the user by its id by given parameters
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <param name="textMaterialParams">Parameters that will be taken into account when returning the text materials</param>
        /// <returns></returns>
        [HttpGet("{id}/textMaterials/saved")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSavedTextMaterials(string id, [FromQuery]TextMaterialParameters textMaterialParams)
        {
            try
            {
                var savedTextMaterials = await _savedTextMaterialService.GetSavedTextMaterialsOfUser(id, textMaterialParams);

                var metadata = new
                {
                    savedTextMaterials.TotalCount,
                    savedTextMaterials.PageSize,
                    savedTextMaterials.CurrentPage,
                    savedTextMaterials.TotalPages,
                    savedTextMaterials.HasNext,
                    savedTextMaterials.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");

                return Ok(savedTextMaterials);
            }
            catch (CardFileException e)
            {
                return BadRequest(e.Message);
            }
           
        }

        /// <summary>
        /// Adds the text material by its id to saved of the user by given id
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <param name="textMaterialId">Id of the text material to add to saved</param>
        /// <returns></returns>
        [HttpPost("{id}/textMaterials/saved")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddToSaved(string id, [FromBody] int textMaterialId)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("User id can't be null");
            }

            try
            {
                await _savedTextMaterialService.AddTextMaterialToSaved(id, textMaterialId);

                return NoContent();
            }
            catch (CardFileException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Removes the text material by its id from saved of the user by given id
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <param name="textMaterialId">Id of the text material to remove from saved</param>
        /// <returns></returns>
        [HttpDelete("{id}/textMaterials/saved")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveFromSaved(string id, [FromBody] int textMaterialId)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("User id can't be null");
            }

            try
            {
                await _savedTextMaterialService.RemoveTextMaterialFromSaved(id, textMaterialId);

                return NoContent();
            }
            catch (CardFileException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Returns liked text materials of the user by its id
        /// </summary>
        /// <param name="id">Id of the user</param>
        [HttpGet("{id}/textMaterials/liked")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLikedTextMaterials(string id)
        {
            try
            {
                var likedTextMaterials = await _likedTextMaterialService.GetLikedTextMaterialsByUserId(id);

                return Ok(likedTextMaterials);
            }
            catch (CardFileException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Adds the text material by its id to liked of the user by given id
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <param name="textMaterialId">Id of the text material to add to liked</param>
        [HttpPost("{id}/textMaterials/liked")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddTextMaterialToLiked(string id, [FromBody] int textMaterialId)
        {
            try
            {
                await _likedTextMaterialService.AddTextMaterialToLiked(id, textMaterialId);

                return NoContent();
            }
            catch (CardFileException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Removes the text material by its id from liked of the user by given id
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <param name="textMaterialId">Id of the text material to remove from liked</param>
        [HttpDelete("{id}/textMaterials/liked")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveTextMaterialFromLiked(string id, [FromBody] int textMaterialId)
        {
            try
            {
                await _likedTextMaterialService.RemoveTextMaterialFromLiked(id, textMaterialId);

                return NoContent();
            }
            catch (CardFileException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Changes the status of receive notifications of the user by its id to the given
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <param name="receiveNotifications">Value to be set on receive notifications of the user</param>
        [HttpPut("{id}/notifications")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ToggleReceiveNotifications(string id, [FromBody] bool receiveNotifications)
        {
            try
            {
                var user = await _userService.ToggleReceiveNotifications(id, receiveNotifications);

                return Ok(user);
            }
            catch (CardFileException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
