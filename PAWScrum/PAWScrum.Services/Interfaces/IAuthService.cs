using PAWScrum.Models.DTOs;
using PAWScrum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWScrum.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User?> LoginAsync(LoginRequest request);
        Task<bool> RegisterAsync(RegisterRequest request);
    }
}
