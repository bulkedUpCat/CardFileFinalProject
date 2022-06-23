using BLL.Services;
using BLL.Validation;
using Core.DTOs;
using Core.Models;
using Core.RequestFeatures;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CardFileApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly TextMaterialService _textMaterialService;

        public UserController(UserService userService,
            TextMaterialService textMaterialService)
        {
            _userService = userService;
            _textMaterialService = textMaterialService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            var users = await _userService.GetAll();

            if (users == null)
            {
                return NotFound("No users were found");
            }

            return Ok(users);
        }

        [HttpGet("{id}")]
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

        [HttpGet("{id}/textMaterials", Name = "GetTextMaterialsByUserId")]
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

        [HttpGet("{id}/textMaterials/saved")]
        public async Task<IActionResult> GetSavedTextMaterials(string id, [FromQuery]TextMaterialParameters textMaterialParams)
        {
            try
            {
                var savedTextMaterials = await _textMaterialService.GetSavedTextMaterialsOfUser(id, textMaterialParams);

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

        [HttpPost("{id}/textMaterials/saved")]
        public async Task<IActionResult> AddToSaved(string id, [FromBody] int textMaterialId)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("User id can't be null");
            }

            try
            {
                await _textMaterialService.AddTextMaterialToSaved(id, textMaterialId);

                return NoContent();
            }
            catch (CardFileException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}/textMaterials/saved")]
        public async Task<IActionResult> RemoveFromSaved(string id, [FromBody] int textMaterialId)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("User id can't be null");
            }

            try
            {
                await _textMaterialService.RemoveTextMaterialFromSaved(id, textMaterialId);

                return NoContent();
            }
            catch (CardFileException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}/notifications")]
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
