using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PAWScrum.Models.DTOs;
using PAWScrum.Models.DTOs.Tasks;

namespace PAWScrum.MVC.Controllers
{
    public class TasksController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "https://localhost:5001/api/tasks"; // Adjust the API URL
        private readonly string _usersApiBaseUrl = "https://localhost:5001/api/users"; // Endpoint to get users

        public TasksController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        // GET: List all tasks
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync(_apiBaseUrl);
            if (!response.IsSuccessStatusCode)
                return View(new List<TaskResponseDto>());

            var json = await response.Content.ReadAsStringAsync();
            var tasks = JsonConvert.DeserializeObject<List<TaskResponseDto>>(json);
            return View(tasks);
        }

        // GET: Task Details
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var task = JsonConvert.DeserializeObject<TaskResponseDto>(json);
            return View(task);
        }

        // GET: Create Task
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create Task
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskCreateDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_apiBaseUrl, content);

            if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            return View(dto);
        }

        // GET: Edit Task
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var task = JsonConvert.DeserializeObject<TaskUpdateDto>(json);
            return View(task);
        }

        // POST: Edit Task
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaskUpdateDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_apiBaseUrl}/{id}", content);

            if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            return View(dto);
        }

        // GET: Delete Task
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var task = JsonConvert.DeserializeObject<TaskResponseDto>(json);
            return View(task);
        }

        // POST: Confirm Delete Task
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _httpClient.DeleteAsync($"{_apiBaseUrl}/{id}");
            return RedirectToAction(nameof(Index));
        }

        // GET: Assign User to Task
        public async Task<IActionResult> Assign(int id)
        {
            // Get task details
            var taskResponse = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");
            if (!taskResponse.IsSuccessStatusCode) return NotFound();

            var taskJson = await taskResponse.Content.ReadAsStringAsync();
            var task = JsonConvert.DeserializeObject<TaskResponseDto>(taskJson);

            // Get user list
            var userResponse = await _httpClient.GetAsync(_usersApiBaseUrl);
            if (!userResponse.IsSuccessStatusCode) return View(task);

            var userJson = await userResponse.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<UserResponse>>(userJson);

            ViewBag.Users = users;
            return View(task);
        }

        // POST: Assign User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(int id, int userId)
        {
            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/{id}/assign/{userId}", null);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            return View();
        }

        // GET: Update Task Hours
        public async Task<IActionResult> UpdateHours(int id)
        {
            // Get task details
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var task = JsonConvert.DeserializeObject<TaskResponseDto>(json);
            return View(task);
        }

        // POST: Update Task Hours
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateHours(int id, int hoursCompleted)
        {
            var content = new StringContent(JsonConvert.SerializeObject(hoursCompleted), Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync($"{_apiBaseUrl}/{id}/hours", content);

            return response.IsSuccessStatusCode ? RedirectToAction(nameof(Index)) : View();
        }
    }
}
