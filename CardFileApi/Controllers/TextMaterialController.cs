using BLL.Services;
using BLL.Validation;
using CardFileApi.Logging;
using Core.DTOs;
using Core.Models;
using Core.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CardFileApi.Controllers
{
    [ApiController]
    [Route("api/textMaterials")]
    public class TextMaterialController : ControllerBase
    {
        private readonly TextMaterialService _textMaterialService;
        private readonly ILoggerManager _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TextMaterialController(TextMaterialService textMaterialService,
            ILoggerManager logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _textMaterialService = textMaterialService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TextMaterialDTO>>> Get([FromQuery]TextMaterialParameters parameters)
        
        {
            var textMaterials = await _textMaterialService.GetTextMaterials(parameters);

            if (textMaterials == null)
            {
                _logger.LogInfo("No text materials were found");
                return NotFound("No text materials were found");
            }

            return Ok(textMaterials);
        }


        [HttpGet("{id}", Name = "GetTextMaterialById")]
        public async Task<ActionResult<TextMaterialDTO>> Get(int id)
        {
            var textMaterial = await _textMaterialService.GetTextMaterialById(id);

            if (textMaterial == null)
            {
                _logger.LogInfo($"Text material with id {id} does not exist");
                return NotFound($"Text material with id {id} does not exist");
            }

            return Ok(textMaterial);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateTextMaterialDTO textMaterialDTO)
        {
            try
            {
                var textMaterial = await _textMaterialService.CreateTextMaterial(textMaterialDTO);

                return CreatedAtRoute("GetTextMaterialById", new { id = textMaterial.Id }, textMaterial);
            }
            catch (CardFileException e)
            {
                _logger.LogInfo($"Failed to create a text material: {e.Message}");
                return BadRequest(e.Message);
            }
        }
        
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{id}/print")]
        public async Task<IActionResult> SendAsPdf(int id,[FromQuery] EmailParameters emailParams)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                await _textMaterialService.SendTextMaterialAsPdf(userId, id, emailParams);

                return Ok();
            }
            catch (CardFileException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Manager")]
        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                await _textMaterialService.ApproveTextMaterial(id);

                return NoContent();
            }
            catch (CardFileException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Manager")]
        [HttpPost("{id}/reject")]
        public async Task<IActionResult> Reject(int id)
        {
            try
            {
                await _textMaterialService.RejectTextMaterial(id);

                return NoContent();
            }
            catch (CardFileException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]UpdateTextMaterialDTO textMaterialDTO)
        {
            try
            {
                await _textMaterialService.UpdateTextMaterial(textMaterialDTO);
                
                return NoContent();
            }
            catch (CardFileException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
