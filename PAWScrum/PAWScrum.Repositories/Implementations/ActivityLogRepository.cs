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
    public class ActivityLogRepository : IActivityLogRepository
    {
        private readonly PAWScrumDbContext _context;

        public ActivityLogRepository(PAWScrumDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ActivityLog>> GetByProjectAsync(int projectId)
        {
            return await _context.ActivityLogs
                .Include(a => a.User)
                .Include(a => a.Project)
                .Where(a => a.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActivityLog>> GetByUserAsync(int userId)
        {
            return await _context.ActivityLogs
                .Include(a => a.Project)
                .Include(a => a.User)
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task<ActivityLog> AddAsync(ActivityLog log)
        {
            _context.ActivityLogs.Add(log);
            await _context.SaveChangesAsync();
            return log;
        }
    }
}
