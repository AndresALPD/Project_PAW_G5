using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAWScrum.Models;
using PAWScrum.Models.DTOs;

namespace PAWScrum.Business.Interfaces
{
    public interface IUserBusiness
    {
        Task<User?> ValidateUserCredentialsAsync(string email, string password);
        Task<bool> CreateUserAsync(RegisterRequest request);
    }
}
