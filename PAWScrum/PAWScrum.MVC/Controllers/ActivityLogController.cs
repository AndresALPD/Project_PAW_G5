using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PAWScrum.Models.DTOs.ActivityLog;

namespace PAWScrum.MVC.Controllers
{
    public class ActivityLogController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "https://localhost:5001/api/activity";

        public ActivityLogController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        // List activity by project
        public async Task<IActionResult> ByProject(int projectId)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/project/{projectId}");
            if (!response.IsSuccessStatusCode)
                return View("Index", new List<ActivityLogResponseDto>());

            var json = await response.Content.ReadAsStringAsync();
            var logs = JsonConvert.DeserializeObject<List<ActivityLogResponseDto>>(json);

            ViewBag.ProjectId = projectId;
            return View("Index", logs);
        }

        // List activity by user
        public async Task<IActionResult> ByUser(int userId)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/user/{userId}");
            if (!response.IsSuccessStatusCode)
                return View("Index", new List<ActivityLogResponseDto>());

            var json = await response.Content.ReadAsStringAsync();
            var logs = JsonConvert.DeserializeObject<List<ActivityLogResponseDto>>(json);

            ViewBag.UserId = userId;
            return View("Index", logs);
        }

        // Show recent activity by project
        public async Task<IActionResult> Recent(int projectId)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/project/{projectId}/recent");
            if (!response.IsSuccessStatusCode)
                return View(new List<ActivityLogResponseDto>());

            var json = await response.Content.ReadAsStringAsync();
            var logs = JsonConvert.DeserializeObject<List<ActivityLogResponseDto>>(json);

            ViewBag.ProjectId = projectId;
            return View("Recent", logs);
        }

        // NonAction to register activity
        [NonAction]
        public async Task RegisterActivityAsync(int userId, int? projectId, string action)
        {
            var log = new ActivityLogCreateDto
            {
                UserId = userId,
                ProjectId = projectId,
                Action = action
            };

            var content = new StringContent(JsonConvert.SerializeObject(log), Encoding.UTF8, "application/json");
            await _httpClient.PostAsync(_apiBaseUrl, content);
        }
    }
}
