using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAWScrum.Models;

namespace PAWScrum.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);
    }
}

