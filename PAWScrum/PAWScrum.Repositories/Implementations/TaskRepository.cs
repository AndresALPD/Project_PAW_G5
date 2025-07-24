using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PAWScrum.Data.Context;
using PAWScrum.Models.Entities;
using PAWScrum.Repositories.Interfaces;

namespace PAWScrum.Repositories.Implementations
{
    public class TaskRepository : ITaskRepository
    {
        private readonly PAWScrumDbContext _context;

        public TaskRepository(PAWScrumDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WorkTask>> GetAllAsync()
            => await _context.WorkTasks.ToListAsync();

        public async Task<WorkTask> GetByIdAsync(int id)
            => await _context.WorkTasks.FindAsync(id);

        public async Task<WorkTask> AddAsync(WorkTask task)
        {
            _context.WorkTasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<WorkTask> UpdateAsync(WorkTask task)
        {
            _context.WorkTasks.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var task = await _context.WorkTasks.FindAsync(id);
            if (task == null) return false;

            _context.WorkTasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
