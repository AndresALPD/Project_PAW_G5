using PAWScrum.Data.Context;
using PAWScrum.Models;
using PAWScrum.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWScrum.Repositories.Implementations
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);

    }
}