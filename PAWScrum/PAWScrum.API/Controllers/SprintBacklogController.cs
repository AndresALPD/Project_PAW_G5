using Microsoft.AspNetCore.Mvc;
using PAWScrum.Business.Interfaces;
using PAWScrum.Models;

namespace PAWScrum.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SprintBacklogController : ControllerBase
    {
        private readonly ISprintBacklogBusiness _business;

        public SprintBacklogController(ISprintBacklogBusiness business)
        {
            _business = business;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SprintBacklogItem>>> GetAll()
        {
            var items = await _business.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SprintBacklogItem>> GetById(int id)
        {
            var item = await _business.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpGet("bysprint/{sprintId}")]
        public async Task<ActionResult<IEnumerable<SprintBacklogItem>>> GetBySprintId(int sprintId)
        {
            var items = await _business.GetBySprintIdAsync(sprintId);
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult> Create(SprintBacklogItem item)
        {
            await _business.CreateAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = item.SprintItemId }, item);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, SprintBacklogItem item)
        {
            if (id != item.SprintItemId)
                return BadRequest();

            await _business.UpdateAsync(item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _business.DeleteAsync(id);
            return NoContent();
        }
    }
}