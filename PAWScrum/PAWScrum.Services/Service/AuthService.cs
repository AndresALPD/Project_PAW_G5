using Microsoft.IdentityModel.Tokens;
using PAWScrum.Business.Interfaces;
using PAWScrum.Models.DTOs;
using PAWScrum.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using PAWScrum.Services.Interfaces;

namespace PAWScrum.Services.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUserBusiness _userBusiness;
        private readonly IConfiguration _configuration;

        public AuthService(IUserBusiness userBusiness, IConfiguration configuration)
        {
            _userBusiness = userBusiness;
            _configuration = configuration;
        }

        public async Task<User?> LoginAsync(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return null;

            var user = await _userBusiness.ValidateUserCredentialsAsync(request.Email, request.Password);
            return user;
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            var result = await _userBusiness.CreateUserAsync(request);
            return result;
        }

        public async Task<string> GenerateSwaggerTokenAsync(string username, string password)
        {
            var user = await _userBusiness.ValidateUserCredentialsAsync(username, password);
            if (user == null)
                return null;

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("swagger", "true") 
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}