using PAWScrum.Models;
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
	}
}
