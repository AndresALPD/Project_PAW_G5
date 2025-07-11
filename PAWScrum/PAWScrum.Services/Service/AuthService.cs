using PAWScrum.Business.Interfaces;
using PAWScrum.Models.DTOs;
using PAWScrum.Models;
using PAWScrum.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWScrum.Services.Service
{
	public class AuthService : IAuthService
	{
		private readonly IUserBusiness _userBusiness;

		public AuthService(IUserBusiness userBusiness)
		{
			_userBusiness = userBusiness;
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

    }
}
