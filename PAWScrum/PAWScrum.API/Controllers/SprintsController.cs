using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWScrum.Data.Context;
using PAWScrum.Models;
using PAWScrum.Models.DTOs.Sprints;

namespace PAWScrum.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SprintsController : ControllerBase
    {
        private readonly PAWScrumDbContext _context;

        public SprintsController(PAWScrumDbContext context)
        {
            _context = context;
        }

        // GET: api/sprints
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SprintDto>>> GetSprints()
        {
            var sprints = await _context.Sprints
                .Select(s => new SprintDto
                {
                    SprintId = s.SprintId,
                    ProjectId = s.ProjectId,
                    Name = s.Name,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    Goal = s.Goal
                }).ToListAsync();

            return Ok(sprints);
        }

        // GET: api/sprints/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SprintDto>> GetSprint(int id)
        {
            var s = await _context.Sprints
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.SprintId == id);

            if (s == null) return NotFound();

            var dto = new SprintDto
            {
                SprintId = s.SprintId,
                ProjectId = s.ProjectId,
                Name = s.Name,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                Goal = s.Goal
            };

            return Ok(dto);
        }

        //POST: api/sprints
        [HttpPost]
        public async Task<ActionResult<SprintDto>> PostSprint(SprintCreateDto dto)
        {
            var sprint = new Sprints
            {
                ProjectId = dto.ProjectId,
                Name = dto.Name,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Goal = dto.Goal
            };

            _context.Sprints.Add(sprint);
            await _context.SaveChangesAsync();

            var result = new SprintDto
            {
                SprintId = sprint.SprintId,
                ProjectId = sprint.ProjectId,
                Name = sprint.Name,
                StartDate = sprint.StartDate,
                EndDate = sprint.EndDate,
                Goal = sprint.Goal
            };

            return CreatedAtAction(nameof(GetSprints), new { id = sprint.SprintId }, result);
        }

        // PUT: api/sprints/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSprint(int id, SprintCreateDto dto)
        {
            var s = await _context.Sprints.FindAsync(id);
            if (s == null) return NotFound();

            s.ProjectId = dto.ProjectId;
            s.Name = dto.Name;
            s.StartDate = dto.StartDate;
            s.EndDate = dto.EndDate;
            s.Goal = dto.Goal;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/sprints/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSprint(int id)
        {
            var sprint = await _context.Sprints.FindAsync(id);
            if (sprint == null) return NotFound();
            _context.Sprints.Remove(sprint);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}