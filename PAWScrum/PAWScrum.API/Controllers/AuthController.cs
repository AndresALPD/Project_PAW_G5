using Microsoft.AspNetCore.Mvc;
using PAWScrum.Services.Interfaces;
using PAWScrum.Models;
using PAWScrum.Models.DTOs;

namespace PAWScrum.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _authService.LoginAsync(request);
            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            return Ok(new
            {
                user.UserId,
                user.Username,
                user.Email,
                user.FirstName,
                user.LastName,
                user.Role
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(request);

            if (!result)
                return Conflict(new { message = "El correo ya está registrado." });

            return Ok(new { message = "Usuario registrado exitosamente." });
        }

        [HttpPost("swagger-token")]
        public async Task<IActionResult> GenerateSwaggerToken([FromBody] SwaggerTokenRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _authService.GenerateSwaggerTokenAsync(request.Username, request.Password);

            if (token == null)
                return Unauthorized(new { message = "Credenciales inválidas" });

            return Ok(new { token });
        }
    }
}