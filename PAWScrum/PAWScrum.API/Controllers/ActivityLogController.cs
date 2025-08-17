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

        public ActivityLogController(IActivityLogService service)
        {
            _service = service;
        }

        // GET api/activity/{projectId}/recent?take=20
        [HttpGet("{projectId:int}/recent")]
        public async Task<IActionResult> Recent(int projectId, [FromQuery] int take = 20)
        {
            var items = await _service.GetRecentAsync(projectId, take);
            return Ok(items); // ✅ no asignar a var
        }

        // GET api/activity/project/{projectId}
        [HttpGet("project/{projectId:int}")]
        public async Task<IActionResult> ByProject(int projectId)
        {
            var items = await _service.GetByProjectAsync(projectId);
            return Ok(items);
        }

        // GET api/activity/user/{userId}
        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> ByUser(int userId)
        {
            var items = await _service.GetByUserAsync(userId);
            return Ok(items);
        }

        // POST api/activity
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ActivityLog log)
        {
            var created = await _service.CreateAsync(log);
            return CreatedAtAction(nameof(Recent), new { projectId = created.ProjectId }, created);
        }

        // DELETE api/activity/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}
