using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWScrum.Data.Context;

namespace PAWScrum.API.Controllers
{
    [ApiController]
    [Route("api/health")]
    public class HealthController : ControllerBase
    {
        private readonly PAWScrumDbContext _ctx;
        public HealthController(PAWScrumDbContext ctx) => _ctx = ctx;

        [HttpGet("db")]
        public async Task<IActionResult> Db()
        {
            var canConnect = await _ctx.Database.CanConnectAsync();
            var tasks = await _ctx.Tasks.AsNoTracking().Take(1).ToListAsync();
            var comments = await _ctx.Comments.AsNoTracking().Take(1).ToListAsync();
            var logs = await _ctx.ActivityLogs.AsNoTracking().Take(1).ToListAsync();

            return Ok(new { canConnect, tasksRows = tasks.Count, commentsRows = comments.Count, activityRows = logs.Count });
        }
    }
}
