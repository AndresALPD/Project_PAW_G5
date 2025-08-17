using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PAWScrum.Data.Context;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PAWScrum.Models.Entities;
using PAWScrum.Repositories.Interfaces;

namespace PAWScrum.Repositories.Implementations
{
    public class ActivityLogRepository : IActivityLogRepository
    {
        private readonly PAWScrumDbContext _ctx;
        public ActivityLogRepository(PAWScrumDbContext ctx) => _ctx = ctx;

        public async Task<IEnumerable<ActivityLog>> GetRecentAsync(int projectId, int take = 20)
            => await _ctx.ActivityLogs.AsNoTracking()
                   .Where(a => a.ProjectId == projectId)
                   .OrderByDescending(a => a.Timestamp)
                   .Take(take)
                   .ToListAsync();

        public async Task<IEnumerable<ActivityLog>> GetByProjectAsync(int projectId)
            => await _ctx.ActivityLogs.AsNoTracking()
                   .Where(a => a.ProjectId == projectId)
                   .OrderByDescending(a => a.Timestamp)
                   .ToListAsync();

        public async Task<IEnumerable<ActivityLog>> GetByUserAsync(int userId)
            => await _ctx.ActivityLogs.AsNoTracking()
                   .Where(a => a.UserId == userId)
                   .OrderByDescending(a => a.Timestamp)
                   .ToListAsync();

        public async Task<ActivityLog> AddAsync(ActivityLog log)
        {
            _ctx.ActivityLogs.Add(log);
            await _ctx.SaveChangesAsync();
            return log;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _ctx.ActivityLogs.FindAsync(id);
            if (entity == null) return false;
            _ctx.ActivityLogs.Remove(entity);
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}