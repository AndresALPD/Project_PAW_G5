using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAWScrum.Models;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Authentication;

namespace PAWScrum.MVC.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProfileController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null) return RedirectToAction("Login", "Account");

            var userId = int.Parse(userIdClaim.Value);
            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.GetAsync($"https://localhost:7250/api/UserAPI/{userId}");
            if (!response.IsSuccessStatusCode) return View(null);

            var json = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<User>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            if (!ModelState.IsValid) return View("Index", user);

            var httpClient = _httpClientFactory.CreateClient();
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync("https://localhost:7250/api/UserAPI", content);
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "There was an error updating your profile.";
                return View("Index", user);
            }

            TempData["EditSuccess"] = "Your profile was updated successfully!";
            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<IActionResult> Delete()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null) return RedirectToAction("Login", "Account");

            var userId = int.Parse(userIdClaim.Value);
            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.DeleteAsync($"https://localhost:7250/api/UserAPI/{userId}");
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "There was an error deleting your account.";
                return RedirectToAction("Index");
            }

            TempData["DeletedMessage"] = "Your account was successfully deleted.";

            await HttpContext.SignOutAsync("PAWScrumAuth");
            return RedirectToAction("Login", "Account");

        }
    }
}
