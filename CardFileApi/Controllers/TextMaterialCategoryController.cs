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

        /// <summary>
        /// Constructor with accepts service to work with text material categories
        /// </summary>
        /// <param name="textMaterialCategoryService">Instance of class that implements ITextMaterialCategory interface to work with text material categories</param>
        public TextMaterialCategoryController(ITextMaterialCategoryService textMaterialCategoryService)
        {
            _textMaterialCategoryService = textMaterialCategoryService;
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
            var category = await _textMaterialCategoryService.CreateTextMaterialCategoryAsync(categoryDTO);

            return CreatedAtRoute("GetCategoryById", new { id = category.Id }, category);
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
            await _textMaterialCategoryService.DeleteTextMaterialCategoryAsync(id);

            return Ok();
        }
    }
}
