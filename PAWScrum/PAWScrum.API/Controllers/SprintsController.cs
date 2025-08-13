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

        [HttpGet("{id}")]
        public async Task<ActionResult<Sprints>> GetSprint(int id)
        {
            var sprint = await _context.Sprints.Include(s => s.Project).FirstOrDefaultAsync(s => s.SprintId == id);
            if (sprint == null) return NotFound();
            return sprint;
        }

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

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSprint(int id, Sprints sprint)
        {
            if (id != sprint.SprintId) return BadRequest();
            _context.Entry(sprint).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

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