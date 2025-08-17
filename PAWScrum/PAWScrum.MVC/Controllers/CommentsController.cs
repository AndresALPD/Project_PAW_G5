using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using PAWScrum.Models.DTOs.Comments;
using PAWScrum.Services.Interfaces;

namespace PAWScrum.MVC.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ICommentService _service;
        public CommentsController(ICommentService service) => _service = service;

        public async Task<IActionResult> Index(int? taskId)
        {
            if (taskId == null) return View(Enumerable.Empty<CommentResponseDto>());
            var items = await _service.GetByTaskAsync(taskId.Value);
            return View(items);
        }

        public IActionResult Create() => View(new CommentCreateDto());

        [HttpPost]
        public async Task<IActionResult> Create(CommentCreateDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _service.CreateAsync(dto);
            return RedirectToAction(nameof(Index), new { taskId = dto.TaskId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();

            ViewBag.CommentId = item.CommentId;
            return View(new CommentCreateDto
            {
                TaskId = item.TaskId,
                UserId = item.UserId,
                Text = item.Text
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CommentCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CommentId = id;
                return View(dto);
            }

            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null) return NotFound();

            return RedirectToAction(nameof(Index), new { taskId = dto.TaskId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int commentId, int taskId)
        {
            await _service.DeleteAsync(commentId);
            return RedirectToAction(nameof(Index), new { taskId });
        }
    }
}
