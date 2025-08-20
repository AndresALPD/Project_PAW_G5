using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PAWScrum.Models.DTOs.Comments;
using PAWScrum.Models.Entities;
using PAWScrum.MVC.Models.Comments;
using PAWScrum.Repositories.Interfaces;
using PAWScrum.Services.Interfaces;

namespace PAWScrum.MVC.Controllers
{
    [Route("[controller]")]
    public class CommentsController : Controller
    {
        private readonly ICommentRepository _repo;
        public CommentsController(ICommentRepository repo) => _repo = repo;

        [HttpGet("")]
        [HttpGet("task/{taskId:int}")]
        public async Task<IActionResult> Index(int? taskId)
        {
            var items = Enumerable.Empty<Comment>();
            if (taskId.HasValue && taskId.Value > 0)
                items = await _repo.GetByTaskAsync(taskId.Value);

            ViewBag.TaskId = taskId ?? 0;
            return View(items);
        }

        [HttpGet("Create")]
        public IActionResult Create(int taskId)
        {
            if (taskId <= 0)
            {
                TempData["Error"] = "Selecciona primero una tarea.";
                return RedirectToAction(nameof(Index));
            }

            var vm = new CommentCreateViewModel { TaskId = taskId };
            return View(vm);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CommentCreateViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var userId = 0;
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(claim) && int.TryParse(claim, out var uid))
                userId = uid;

            var entity = new Comment
            {
                TaskId = vm.TaskId,
                UserId = userId > 0 ? userId : vm.UserId,
                Content = vm.Content,      
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(entity);
            return RedirectToAction(nameof(Index), new { taskId = vm.TaskId });
        }
    }
}