using BLL.Abstractions.cs.Interfaces;
using BLL.Services;
using BLL.Validation;
using CardFileApi.Logging;
using Core.DTOs;
using DAL.Abstractions.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading.Tasks;

namespace CardFileApi.Controllers
{
    /// <summary>
    /// Controller that provides endpoints for working with text material categories
    /// </summary>
    [ApiVersion("1.0")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Route("api/textMaterials/categories")]
    public class TextMaterialCategoryController : ControllerBase
    {
        private readonly ITextMaterialCategoryService _textMaterialCategoryService;
        private readonly ILoggerManager _logger;

        /// <summary>
        /// Constructor with accepts service to work with text material categories and logger to log information
        /// </summary>
        /// <param name="textMaterialCategoryService">Instance of class that implements ITextMaterialCategory interface to work with text material categories</param>
        /// <param name="logger">Instance of class that implements ILoggerManager interface to log the information</param>
        public TextMaterialCategoryController(ITextMaterialCategoryService textMaterialCategoryService,
            ILoggerManager logger)
        {
            _textMaterialCategoryService = textMaterialCategoryService;
            _logger = logger;
        }

        /// <summary>
        /// Returns all existing text material categories from the database
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Returns single text material category with given id
        /// </summary>
        /// <param name="id">Id of the text material category to return</param>
        [HttpGet("{id}", Name = "GetCategoryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Adds new text material category to the database
        /// </summary>
        /// <param name="categoryDTO">Data transfer object that contains data of the new text material category</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostCategory(CreateTextMaterialCategoryDTO categoryDTO)
        {
            try
            {
                var category = await _textMaterialCategoryService.CreateTextMaterialCategoryAsync(categoryDTO);

                return CreatedAtRoute("GetCategoryById", new { id = category.Id }, category);
            }
            catch (CardFileException e)
            {
                _logger.LogInfo($"Failed to create a category with title {categoryDTO.Title}: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Removes existing text material category from the database by its id
        /// </summary>
        /// <param name="id">Id of text material category to remove from the database</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _textMaterialCategoryService.DeleteTextMaterialCategoryAsync(id);

                return Ok();
            }
            catch (CardFileException e)
            {
                _logger.LogInfo($"Failed to delete a category with id {id}: {e.Message}");
                return BadRequest(e.Message);
            }
        }
    }
}
