using Microsoft.AspNetCore.Mvc;
using PAWScrum.Models.DTOs.ActivityLog;
using PAWScrum.Services.Interfaces;

namespace PAWScrum.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityLogController : ControllerBase
    {
        private readonly IActivityLogService _service;

        public ActivityLogController(IActivityLogService service)
        {
            _service = service;
        }

        // GET: api/activitylog/project/{projectId}
        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetByProject(int projectId)
        {
            var logs = await _service.GetByProjectAsync(projectId);
            return Ok(logs);
        }

        // GET: api/activitylog/project/{projectId}/recent
        [HttpGet("project/{projectId}/recent")]
        public async Task<IActionResult> GetRecentByProject(int projectId, [FromQuery] int top = 10)
        {
            var logs = await _service.GetByProjectAsync(projectId);
            var recent = logs.OrderByDescending(l => l.Timestamp).Take(top);
            return Ok(recent);
        }

        // GET: api/activitylog/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var logs = await _service.GetByUserAsync(userId);
            return Ok(logs);
        }

        // POST: api/activitylog
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ActivityLogCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return Ok(created);
        }
    }
}
