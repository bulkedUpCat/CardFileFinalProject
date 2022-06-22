using BLL.Services;
using BLL.Validation;
using Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CardFileApi.Controllers
{
    [ApiController]
    [Route("api/comments")]
    public class CommentController : ControllerBase
    {
        private readonly CommentService _commentService;

        public CommentController(CommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("textMaterials/{id}")]
        public async Task<IActionResult> GetByTextMaterialId(int id)
        {
            try
            {
                var comments = await _commentService.GetCommentsOfTextMaterial(id);

                return Ok(comments);
            }
            catch (CardFileException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCommentDTO commentDTO)
        {
            try
            {
                var comment = await _commentService.CreateComment(commentDTO);

                return Ok(comment);
            }
            catch (CardFileException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateCommentDTO commentDTO)
        {
            try
            {
                var comment = await _commentService.UpdateComment(commentDTO);

                return NoContent();
            }
            catch (CardFileException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _commentService.DeleteComment(id);

                return NoContent();
            }
            catch (CardFileException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
