using BLL.Services;
using BLL.Validation;
using CardFileApi.Logging;
using Core.DTOs;
using DAL.Abstractions.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CardFileApi.Controllers
{
    [ApiController]
    [Route("textMaterials/categories")]
    public class TextMaterialCategoryController : ControllerBase
    {
        private readonly TextMaterialCategoryService _textMaterialCategoryService;
        private readonly ILoggerManager _logger;

        public TextMaterialCategoryController(TextMaterialCategoryService textMaterialCategoryService,
            ILoggerManager logger)
        {
            _textMaterialCategoryService = textMaterialCategoryService;
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _textMaterialCategoryService.GetTextMaterialCategoriesAsync();

            if (categories == null)
            {
                _logger.LogInfo("No categories were found");
                return NotFound("No categories were found");
            }

            return Ok(categories);
        }

        [HttpGet("{id}", Name = "GetCategoryById")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _textMaterialCategoryService.GetTextMaterialCategoryById(id);

            if (category == null)
            {
                _logger.LogInfo($"Failed to find a category with id {id}");
                return NotFound($"Failed to find a category with id {id}");
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> PostCategory(CreateTextMaterialCategoryDTO categoryDTO)
        {
            try
            {
                var category = await _textMaterialCategoryService.CreateTextMaterialCategoryAsync(categoryDTO);

                return CreatedAtRoute("GetCategoryById", new { id = category.Id }, category);
            }
            catch (CardFileException e)
            {
                _logger.LogInfo($"Failed to create a category: {e.Message}");
                return BadRequest(e.Message);
            }
        }
    }
}
