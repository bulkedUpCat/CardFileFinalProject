using BLL.Services;
using BLL.Validation;
using Core.DTOs;
using Core.Models;
using Core.RequestFeatures;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CardFileApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly TextMaterialService _textMaterialService;

        public UserController(TextMaterialService textMaterialService)
        {
            _textMaterialService = textMaterialService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(string id)
        {
            return Ok();
        }

        [HttpGet("{id}/textMaterials", Name = "GetTextMaterialsByUserId")]
        public async Task<ActionResult<IEnumerable<TextMaterialDTO>>> Get(string id,[FromQuery]TextMaterialParameters textMaterialParams)
        {
            try
            {
                var textMaterials = await _textMaterialService.GetTextMaterialsOfUser(id,textMaterialParams);

                return Ok(textMaterials);
            }
            catch (CardFileException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
