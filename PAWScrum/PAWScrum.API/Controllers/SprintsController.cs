using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWScrum.Data.Context;
using PAWScrum.Models;

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

        // GET: api/Sprints
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sprints>>> GetSprints()
        {
            return await _context.Sprints
                                 .Include(s => s.Project)
                                 .ToListAsync();
        }

        // GET: api/Sprints/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sprints>> GetSprint(int id)
        {
            var sprint = await _context.Sprints
                                       .Include(s => s.Project)
                                       .FirstOrDefaultAsync(s => s.SprintId == id);

            if (sprint == null)
            {
                return NotFound();
            }

            return sprint;
        }

        // POST: api/Sprints
        [HttpPost]
        public async Task<ActionResult<Sprints>> PostSprint(Sprints sprint)
        {
            if (string.IsNullOrWhiteSpace(sprint.Name))
            {
                return BadRequest("El nombre del sprint es obligatorio.");
            }

            _context.Sprints.Add(sprint);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSprint), new { id = sprint.SprintId }, sprint);
        }

        // PUT: api/Sprints/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSprint(int id, Sprints sprint)
        {
            if (id != sprint.SprintId)
            {
                return BadRequest("El ID del sprint no coincide.");
            }

            _context.Entry(sprint).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SprintExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Sprints/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSprint(int id)
        {
            var sprint = await _context.Sprints.FindAsync(id);
            if (sprint == null)
            {
                return NotFound();
            }

            _context.Sprints.Remove(sprint);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SprintExists(int id)
        {
            return _context.Sprints.Any(e => e.SprintId == id);
        }
    }
}