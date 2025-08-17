using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PAWScrum.Data.Context;
using PAWScrum.Models;
using PAWScrum.Repositories.Interfaces;

namespace PAWScrum.Repositories.Implementations
{
    public class TaskRepository : ITaskRepository
    
        {
        private readonly PAWScrumDbContext _context;
        public TaskRepository(PAWScrumDbContext context) => _context = context;

        public async Task<IEnumerable<UserTask>> GetAllAsync()
            => await _context.Tasks
                .Include(t => t.AssignedToNavigation)
                .ToListAsync();


        public async Task<UserTask> GetByIdAsync(int id)
            => await _context.Tasks
                .Include(t => t.AssignedToNavigation)
                .FirstOrDefaultAsync(t => t.TaskId == id);

        public async Task<UserTask> AddAsync(UserTask task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<UserTask> UpdateAsync(UserTask task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Tasks.FindAsync(id);
            if (entity == null) return false;
            _context.Tasks.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserTask> AssignUserAsync(int taskId, int userId)
        {
            var entity = await _context.Tasks.FindAsync(taskId);
            if (entity == null) return null;
            entity.AssignedTo = userId;
            _context.Tasks.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<UserTask> UpdateHoursAsync(int taskId, decimal hoursCompleted)
        {
            var entity = await _context.Tasks.FindAsync(taskId);
            if (entity == null) return null;
            entity.CompletedHours = hoursCompleted;
            _context.Tasks.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }


    }
}
