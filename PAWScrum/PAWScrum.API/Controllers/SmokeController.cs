using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWScrum.Data.Context;

namespace PAWScrum.API.Controllers
{
    [ApiController]
    [Route("api/smoke")]
    public class SmokeController : ControllerBase
    {
        private readonly PAWScrumDbContext _ctx;
        public SmokeController(PAWScrumDbContext ctx) => _ctx = ctx;

        // 1) ¿La DB responde?
        [HttpGet("db")]
        public async Task<IActionResult> Db()
        {
            var ok = await _ctx.Database.CanConnectAsync();
            return Ok(new { canConnect = ok });
        }

        // 2) Lista solo IDs de Tasks (sin navegar nada)
        [HttpGet("tasks")]
        public async Task<IActionResult> Tasks()
        {
            var data = await _ctx.Tasks
                .AsNoTracking()
                .Select(t => new { t.TaskId, t.Title, t.Status })  // <- sin navegar
                .Take(50)
                .ToListAsync();

            return Ok(data);
        }

        // 3) Comments por Task SIN incluir User ni Task (evitar navs)
        [HttpGet("comments-by-task/{taskId:int}")]
        public async Task<IActionResult> CommentsByTask(int taskId)
        {
            var data = await _ctx.Comments
                .AsNoTracking()
                .Where(c => c.TaskId == taskId)
                .Select(c => new { c.CommentId, c.TaskId, c.UserId, c.Text, c.CreatedAt })
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();

            return Ok(data);
        }
    }
}
