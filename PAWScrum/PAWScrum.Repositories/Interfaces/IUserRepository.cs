using PAWScrum.Models;
using PAWScrum.Repositories.Implementations;
using PAWScrum.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PAWScrum.Repositories.Interfaces
{
	public class UserRepository : IUserRepository
	{
		private readonly PAWScrumDbContext _context;

		public UserRepository(PAWScrumDbContext context)
		{
			_context = context;
		}

		public async Task<User?> GetUserByEmailAsync(string email)
		{
			return await _context.Users
				.FirstOrDefaultAsync(u => u.Email == email);
		}
	}
}
