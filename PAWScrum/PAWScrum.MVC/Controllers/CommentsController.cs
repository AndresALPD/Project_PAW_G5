using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PAWScrum.Models.DTOs.Comments;

namespace PAWScrum.MVC.Controllers
{
    public class CommentsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ActivityLogController _activityLog;
        private readonly string _apiBaseUrl = "https://localhost:5001/api/comments"; // Ajusta la URL

        public CommentsController(IHttpClientFactory httpClientFactory, ActivityLogController activityLog)
        {
            _httpClient = httpClientFactory.CreateClient();
            _activityLog = activityLog;
        }

        // GET: List comments for a specific task
        public async Task<IActionResult> Index(int taskId)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/task/{taskId}");
            if (!response.IsSuccessStatusCode)
                return View(new List<CommentResponseDto>());

            var json = await response.Content.ReadAsStringAsync();
            var comments = JsonConvert.DeserializeObject<List<CommentResponseDto>>(json);

            ViewBag.TaskId = taskId;
            return View(comments);
        }

        // GET: Create a comment
        public IActionResult Create(int taskId)
        {
            ViewBag.TaskId = taskId;
            return View();
        }

        // POST: Create a comment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CommentCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TaskId = dto.WorkTaskId;
                return View(dto);
            }

            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_apiBaseUrl, content);

            if (response.IsSuccessStatusCode)
            {
                // Registrar en la bitácora
                await _activityLog.RegisterActivityAsync(dto.UserId, null, $"Created a comment on Task {dto.WorkTaskId}");
                return RedirectToAction(nameof(Index), new { taskId = dto.WorkTaskId });
            }

            ViewBag.TaskId = dto.WorkTaskId;
            return View(dto);
        }

        // GET: Edit comment
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var comment = JsonConvert.DeserializeObject<CommentCreateDto>(json);
            return View(comment);
        }

        // POST: Edit comment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CommentCreateDto dto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_apiBaseUrl}/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                // Registrar en la bitácora
                await _activityLog.RegisterActivityAsync(dto.UserId, null, $"Edited a comment (ID {id}) on Task {dto.WorkTaskId}");
                return RedirectToAction(nameof(Index), new { taskId = dto.WorkTaskId });
            }

            return View(dto);
        }

        // GET: Delete comment
        public async Task<IActionResult> Delete(int id, int taskId)
        {
            ViewBag.TaskId = taskId;
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var comment = JsonConvert.DeserializeObject<CommentResponseDto>(json);
            return View(comment);
        }

        // POST: Confirm delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int taskId)
        {
            var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                // Registrar en la bitácora
                await _activityLog.RegisterActivityAsync(1, null, $"Deleted a comment (ID {id}) on Task {taskId}");
                return RedirectToAction(nameof(Index), new { taskId });
            }

            return RedirectToAction(nameof(Index), new { taskId });
        }
    }
}
