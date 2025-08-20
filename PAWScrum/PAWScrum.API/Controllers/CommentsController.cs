using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAWScrum.Models.DTOs.Comments;
using PAWScrum.Services.Interfaces;

namespace PAWScrum.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _service;
        public CommentsController(ICommentService service) => _service = service;

        // GET api/comments/task/1
        [HttpGet("task/{taskId:int}")]
        public async Task<IActionResult> GetByTask(int taskId)
        {
            var items = await _service.GetByTaskAsync(taskId);
            return Ok(items);
        }

        // GET api/comments/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _service.GetByIdAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        // POST api/comments
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CommentCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);

            // Si tu CommentResponseDto expone WorkTaskId, usa eso. Si expone TaskId, cámbialo aquí.
            return CreatedAtAction(nameof(GetByTask), new { taskId = created.TaskId }, created);
        }

        // PUT api/comments/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CommentCreateDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated == null ? NotFound() : Ok(updated);
        }

        // DELETE api/comments/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}