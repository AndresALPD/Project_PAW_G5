using Microsoft.AspNetCore.Mvc;
using PAWScrum.Models.DTOs.Tasks;
using PAWScrum.Services.Interfaces;


namespace PAWScrum.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IUserService _userService;

        public TasksController(ITaskService taskService, IUserService userService)
        {
            _taskService = taskService;
            _userService = userService;
        }

        // GET /api/Tasks
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _taskService.GetAllAsync();
            return Ok(items);
        }

        // PATCH /api/Tasks/{id}/hours
    
        [HttpPatch("{id:int}/hours")]
        public async Task<IActionResult> UpdateHours(int id, [FromBody] decimal hoursCompleted)
        {
            var exists = await _taskService.ExistsAsync(id);
            if (!exists) return NotFound($"Task {id} not found");

            var ok = await _taskService.UpdateHoursAsync(id, hoursCompleted);
            return ok ? NoContent() : StatusCode(500, "Could not update hours");
        }

        

        // POST /api/Tasks/{taskId}/assign/{userId}
        [HttpPost("{taskId:int}/assign/{userId:int}")]
        public async Task<IActionResult> AssignUser(int taskId, int userId)
        {
            var task = await _taskService.GetByIdAsync(taskId);
            if (task == null) return NotFound($"Task {taskId} not found");

            var user = await _userService.GetByIdAsync(userId);
            if (user == null) return NotFound($"User {userId} not found");

            var ok = await _taskService.AssignUserAsync(taskId, userId);
            return ok ? NoContent() : StatusCode(500, "Could not assign user");
        }

        // GET /api/tasks/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var task = await _taskService.GetByIdAsync(id);
            return task == null ? NotFound() : Ok(task);
        }

        // POST /api/tasks
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskCreateDto dto)
        {
            var created = await _taskService.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        // PUT /api/tasks/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] TaskUpdateDto dto)
        {
            var updated = await _taskService.UpdateAsync(id, dto);
            return updated == null ? NotFound() : Ok(updated);
        }

        // DELETE /api/tasks/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _taskService.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }

    }
}
