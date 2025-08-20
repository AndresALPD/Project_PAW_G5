using Microsoft.AspNetCore.Mvc;
using PAWScrum.Models.DTOs.ActivityLog;
using PAWScrum.Models.Entities;
using PAWScrum.Services.Interfaces;

namespace PAWScrum.API.Controllers
{
    [ApiController]
    [Route("api/activity")]
    public class ActivityLogController : ControllerBase
    {
        private readonly IActivityLogService _service;

        public ActivityLogController(IActivityLogService service) => _service = service;

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ActivityLog>> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpGet("{projectId:int}/recent")]
        public async Task<IActionResult> Recent(int projectId, [FromQuery] int take = 20)
        {
            var items = await _service.GetRecentAsync(projectId, take);
            return Ok(items);
        }

        [HttpGet("project/{projectId:int}")]
        public async Task<IActionResult> ByProject(int projectId)
        {
            var items = await _service.GetByProjectAsync(projectId);
            return Ok(items);
        }

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> ByUser(int userId)
        {
            var items = await _service.GetByUserAsync(userId);
            return Ok(items);
        }

        // POST api/activity  (recibe DTO plano)
        [HttpPost]
        public async Task<ActionResult<ActivityLog>> Create([FromBody] ActivityLogCreateDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var entity = new ActivityLog
            {
                UserId = dto.UserId,
                ProjectId = dto.ProjectId,
                Action = dto.Action,
                Timestamp = dto.Timestamp ?? System.DateTime.UtcNow
            };

            var created = await _service.CreateAsync(entity); // <-- devuelve ActivityLog
            return CreatedAtAction(nameof(GetById), new { id = created.ActivityId }, created);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}