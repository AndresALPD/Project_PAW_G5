using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWScrum.Business.Interfaces;
using PAWScrum.Data.Context;
using PAWScrum.Models;

namespace PAWScrum.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductBacklogController : ControllerBase
    {
        private readonly IProductBacklogBusiness _business;

        public ProductBacklogController(IProductBacklogBusiness business)
        {
            _business = business;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _business.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _business.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetByProjectId(int projectId)
        {
            var items = await _business.GetByProjectIdAsync(projectId);
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductBacklogItem item)
        {
            await _business.AddAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = item.ItemId }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductBacklogItem item)
        {
            if (id != item.ItemId)
                return BadRequest("ID mismatch");

            await _business.UpdateAsync(item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _business.DeleteAsync(id);
            return NoContent();
        }
    }
}