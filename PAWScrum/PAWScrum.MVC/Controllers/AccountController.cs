using PAWScrum.Architecture.Interfaces;
using PAWScrum.Architecture.Providers;
using Microsoft.AspNetCore.Mvc;
using PAWScrum.Models;
using PAWScrum.Models.DTOs;
using PAWScrum.Architecture;
using System.Text.Json;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace PAWScrum.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRestProvider _restProvider;

        public AccountController(IRestProvider restProvider)
        {
            _restProvider = restProvider;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            if (!ModelState.IsValid)
                return View(model);

            using var httpClient = new HttpClient();
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://localhost:7250/api/auth/login", content);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Invalid credentials.";
                return View(model);
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<UserResponse>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Create claims for the session
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("UserId", user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var claimsIdentity = new ClaimsIdentity(claims, "PAWScrumAuth");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync("PAWScrumAuth", claimsPrincipal);

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("PAWScrumAuth");
            return RedirectToAction("Login", "Account");
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            if (!ModelState.IsValid)
                return View(model);

            using var httpClient = new HttpClient();
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://localhost:7250/api/auth/register", content);

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                ViewBag.Error = "This email is already registered.";
                return View(model);
            }

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = $"Error during registration: {(int)response.StatusCode} - {response.StatusCode}";
                return View(model);
            }

            TempData["RegisterSuccess"] = "User successfully registered!";
            ModelState.Clear();
            return View("Login");
        }
    }
}
