using PAWScrum.Business.Interfaces;
using PAWScrum.Models;
using PAWScrum.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWScrum.Services.Service
{
    public class UserService : IUserService
    {
        private readonly IUserBusiness _userBusiness;

        public UserService(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _userBusiness.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(User user)
        {
            return await _userBusiness.UpdateAsync(user);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _userBusiness.DeleteAsync(id);
        }
    }
}
