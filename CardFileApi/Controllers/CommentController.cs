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
    [ApiVersion("1.0")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Route("api/comments")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        /// <summary>
        /// Constructor that accepts comment service to work with comments
        /// </summary>
        /// <param name="commentService">Instance of the class which implements ICommentService interface</param>
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
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
            var comments = await _commentService.GetCommentsOfTextMaterial(id);

            return Ok(comments);
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
            var comment = await _commentService.CreateComment(commentDTO);

            return Ok(comment);
        }

        /// <summary>
        /// Updates an existing comment in the database
        /// </summary>
        /// <param name="commentDTO">Data transfer object that contains data of the existing comment to update</param>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] UpdateCommentDTO commentDTO)
        {
            var comment = await _commentService.UpdateComment(commentDTO);

            return Ok("Comment updated");
        }

        /// <summary>
        /// Removes an existing comment from the database by its id
        /// </summary>
        /// <param name="id">Id of the existing comment to delete</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            await _commentService.DeleteComment(id);

            return Ok("Comment deleted");
        }
    }
}
