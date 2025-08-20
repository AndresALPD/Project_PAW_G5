using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWScrum.Business.Interfaces;
using PAWScrum.Data.Context;
using PAWScrum.Models;
using PAWScrum.Models.DTOs.ProductBacklog;

namespace PAWScrum.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductBacklogController : ControllerBase
    {
        private readonly PAWScrumDbContext _context;

        public ProductBacklogController(PAWScrumDbContext context)
        {
            _context = context;
        }

        // GET: api/productbacklog
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductBacklogDto>>> GetAll()
        {
            var data = await _context.ProductBacklogItems
                .Select(p => new ProductBacklogDto
                {
                    ItemId = p.ItemId,
                    ProjectId = p.ProjectId,
                    Title = p.Title,
                    Description = p.Description,
                    Priority = p.Priority ?? 1,
                    Status = p.Status ?? "To Do",
                    CreatedAt = p.CreatedAt ?? DateTime.Now
                })
                .ToListAsync();

            return Ok(data);
        }

        // GET: api/productbacklog/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductBacklogDto>> GetById(int id)
        {
            var p = await _context.ProductBacklogItems.FirstOrDefaultAsync(x => x.ItemId == id);
            if (p == null) return NotFound();

            return new ProductBacklogDto
            {
                ItemId = p.ItemId,
                ProjectId = p.ProjectId,
                Title = p.Title,
                Description = p.Description,
                Priority = p.Priority ?? 1,
                Status = p.Status ?? "To Do",
                CreatedAt = p.CreatedAt ?? DateTime.Now
            };
        }

        // POST: api/productbacklog
        [HttpPost]
        public async Task<ActionResult<ProductBacklogDto>> Create(ProductBacklogCreateDto dto)
        {
            if (dto.Priority < 1 || dto.Priority > 5)
                return BadRequest("Priority must be between 1 (lowest) and 5 (highest).");

            var allowedStatus = new[] { "To Do", "In Progress", "Done" };
            if (!allowedStatus.Contains(dto.Status))
                return BadRequest($"Status must be one of: {string.Join(", ", allowedStatus)}.");

            var entity = new ProductBacklogItem
            {
                ProjectId = dto.ProjectId,
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,  
                Status = dto.Status,
                CreatedAt = DateTime.UtcNow
            };

            _context.ProductBacklogItems.Add(entity);
            await _context.SaveChangesAsync();

            var result = new ProductBacklogDto
            {
                ItemId = entity.ItemId,
                ProjectId = entity.ProjectId,
                Title = entity.Title,
                Description = entity.Description,
                Priority = entity.Priority ?? 1,
                Status = entity.Status ?? "To Do",
                CreatedAt = entity.CreatedAt ?? DateTime.Now
            };

            return CreatedAtAction(nameof(GetById), new { id = entity.ItemId }, result);
        }

        // PUT: api/productbacklog/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductBacklogCreateDto dto)
        {
            if (dto.Priority < 1 || dto.Priority > 5)
                return BadRequest("Priority must be between 1 (lowest) and 5 (highest).");

            var allowedStatus = new[] { "To Do", "In Progress", "Done" };
            if (!allowedStatus.Contains(dto.Status))
                return BadRequest($"Status must be one of: {string.Join(", ", allowedStatus)}.");

            var entity = await _context.ProductBacklogItems.FindAsync(id);
            if (entity == null) return NotFound();

            entity.ProjectId = dto.ProjectId;
            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.Priority = dto.Priority;
            entity.Status = dto.Status;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/productbacklog/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.ProductBacklogItems.FindAsync(id);
            if (entity == null) return NotFound();

            _context.ProductBacklogItems.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}