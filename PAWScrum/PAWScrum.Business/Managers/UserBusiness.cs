using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAWScrum.Business.Interfaces;
using PAWScrum.Models;
using PAWScrum.Repositories.Interfaces;
using PAWScrum.Architecture.Helpers;
using PAWScrum.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using PAWScrum.Models.DTOs;
using PAWScrum.Data.Context;

namespace PAWScrum.Business.Managers
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IUserRepository _userRepository;
        private readonly PAWScrumDbContext _context;

        public UserBusiness(IUserRepository userRepository, PAWScrumDbContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        public async Task<User?> ValidateUserCredentialsAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null) return null;

            var isValid = PasswordHelper.VerifyPassword(password, user.PasswordHash);
            return isValid ? user : null;
        }

        public async Task<bool> CreateUserAsync(RegisterRequest request)
        {

            var existing = await _userRepository.GetUserByEmailAsync(request.Email);
            if (existing != null)
                return false;

            var newUser = new User
            {
                Email = request.Email,
                Username = request.Username,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Role = request.Role,
                PasswordHash = PasswordHelper.HashPassword(request.Password)
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(User user)
        {
            var existingUser = await _userRepository.GetByIdAsync(user.UserId);
            if (existingUser == null)
                return false;

            // Solo actualizamos los campos editables (no se toca el PasswordHash a menos que se quiera permitir)
            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Role = user.Role;

            return await _userRepository.UpdateAsync(existingUser);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return false;

            return await _userRepository.DeleteAsync(id);
        }
    }
}
