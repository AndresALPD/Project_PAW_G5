using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWScrum.Business.Interfaces;
using PAWScrum.Data.Context;
using PAWScrum.Models;
using PAWScrum.Models.DTOs.SprintBacklog;

namespace PAWScrum.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SprintBacklogController : ControllerBase
    {
        private readonly PAWScrumDbContext _context;

        public SprintBacklogController(PAWScrumDbContext context)
        {
            _context = context;
        }

        // GET: api/sprintbacklog
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SprintBacklogDto>>> GetAll()
        {
            var list = await _context.SprintBacklogItems
                .AsNoTracking()
                .Select(x => new SprintBacklogDto
                {
                    SprintItemId = x.SprintItemId,
                    SprintId = x.SprintId,
                    ProductBacklogItemId = x.ProductBacklogItemId,
                    AssignedTo = x.AssignedTo,
                    Status = x.Status ?? "To Do",
                    EstimationHours = x.EstimationHours,
                    CompletedHours = x.CompletedHours
                })
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/sprintbacklog/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SprintBacklogDto>> GetById(int id)
        {
            var x = await _context.SprintBacklogItems
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SprintItemId == id);

            if (x == null) return NotFound();

            return new SprintBacklogDto
            {
                SprintItemId = x.SprintItemId,
                SprintId = x.SprintId,
                ProductBacklogItemId = x.ProductBacklogItemId,
                AssignedTo = x.AssignedTo,
                Status = x.Status ?? "To Do",
                EstimationHours = x.EstimationHours,
                CompletedHours = x.CompletedHours
            };
        }

        // POST: api/sprintbacklog
        [HttpPost]
        public async Task<ActionResult<SprintBacklogDto>> Create(SprintBacklogCreateDto dto)
        {
            var entity = new SprintBacklogItem
            {
                SprintId = dto.SprintId,
                ProductBacklogItemId = dto.ProductBacklogItemId,
                AssignedTo = dto.AssignedTo,
                Status = dto.Status,
                EstimationHours = dto.EstimationHours,
                CompletedHours = dto.CompletedHours
            };

            _context.SprintBacklogItems.Add(entity);
            await _context.SaveChangesAsync();
            return Ok(entity);

           
        }

        // PUT: api/sprintbacklog/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SprintBacklogCreateDto dto)
        {
            var entity = await _context.SprintBacklogItems.FindAsync(id);
            if (entity == null) return NotFound();

            entity.SprintId = dto.SprintId;
            entity.ProductBacklogItemId = dto.ProductBacklogItemId;
            entity.AssignedTo = dto.AssignedTo;
            entity.Status = dto.Status;
            entity.EstimationHours = dto.EstimationHours;
            entity.CompletedHours = dto.CompletedHours;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/sprintbacklog/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.SprintBacklogItems.FindAsync(id);
            if (entity == null) return NotFound();

            _context.SprintBacklogItems.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}