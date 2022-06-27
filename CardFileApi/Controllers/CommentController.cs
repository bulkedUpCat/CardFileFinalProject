using BLL.Abstractions.cs.Interfaces;
using BLL.Services;
using BLL.Validation;
using CardFileApi.Logging;
using Core.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CardFileApi.Controllers
{
    /// <summary>
    /// Controller that provides endpoints for working with comments
    /// </summary>
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Route("api/comments")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly ILoggerManager _logger;

        /// <summary>
        /// Constructor with two parameters
        /// </summary>
        /// <param name="commentService">Instance of the class which implements ICommentService interface</param>
        /// <param name="logger">Instance of class that implements ILoggerManager interface to log information</param>
        public CommentController(ICommentService commentService,
            ILoggerManager logger)
        {
            _commentService = commentService;
            _logger = logger;
        }

        /// <summary>
        /// Finds the comment by the text material id
        /// </summary>
        /// <param name="id">Id of the text material</param>
        /// <returns>All comments which belong to the text material with given id</returns>
        [HttpGet("textMaterials/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByTextMaterialId(int id)
        {
            try
            {
                var comments = await _commentService.GetCommentsOfTextMaterial(id);

                return Ok(comments);
            }
            catch (CardFileException e)
            {
                _logger.LogInfo($"While getting comments by text material id {id}: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Adds a new comment to the database
        /// </summary>
        /// <param name="commentDTO">Data transfer object that contains data for the model</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] CreateCommentDTO commentDTO)
        {
            try
            {
                var comment = await _commentService.CreateComment(commentDTO);

                return Ok(comment);
            }
            catch (CardFileException e)
            {
                _logger.LogInfo($"While creating a new comment with author id {commentDTO.UserId} and text material id {commentDTO.TextMaterialId}: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Updates an existing comment in the database
        /// </summary>
        /// <param name="commentDTO">Data transfer object that contains data of the existing comment to update</param>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] UpdateCommentDTO commentDTO)
        {
            try
            {
                var comment = await _commentService.UpdateComment(commentDTO);

                return NoContent();
            }
            catch (CardFileException e)
            {
                _logger.LogInfo($"While updating a comment with id {commentDTO.Id}: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Removes an existing comment from the database by its id
        /// </summary>
        /// <param name="id">Id of the existing comment to delete</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _commentService.DeleteComment(id);

                return NoContent();
            }
            catch (CardFileException e)
            {
                _logger.LogInfo($"While deleting a comment with id {id}: {e.Message}");
                return BadRequest(e.Message);
            }
        }
    }
}
