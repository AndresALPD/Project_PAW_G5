using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using PAWScrum.Models.DTOs.Comments;
using PAWScrum.Services.Interfaces;

namespace PAWScrum.MVC.Controllers
{
    public class CommentsController : Controller
    {
        private readonly HttpClient _client;
        private readonly string _api = "https://localhost:5001/api/comments";

        public CommentsController(IHttpClientFactory f) => _client = f.CreateClient();

        public async Task<IActionResult> Index(int? taskId)
        {
            if (taskId == null) return View(Enumerable.Empty<CommentResponseDto>());
            var res = await _client.GetAsync($"{_api}/task/{taskId.Value}");
            if (!res.IsSuccessStatusCode) return View(Enumerable.Empty<CommentResponseDto>());
            var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CommentResponseDto>>(await res.Content.ReadAsStringAsync());
            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CommentCreateDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            await _client.PostAsync(_api, content);
            return RedirectToAction(nameof(Index), new { taskId = dto.TaskId });
        }
    }

}
