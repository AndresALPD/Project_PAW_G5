using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;          
using PAWScrum.Data;                          
using PAWScrum.Data.Context;
using PAWScrum.Models.Entities;               
using PAWScrum.Repositories.Interfaces;       

namespace PAWScrum.Repositories.Implementations
{
    public class ActivityLogRepository : IActivityLogRepository
    {
        private readonly PAWScrumDbContext _ctx;
        public ActivityLogRepository(PAWScrumDbContext ctx) => _ctx = ctx;

        private IQueryable<ActivityLog> Query =>
            _ctx.Set<ActivityLog>().AsNoTracking();

        public Task<ActivityLog?> GetByIdAsync(int id) =>
            Query.FirstOrDefaultAsync(a => a.ActivityId == id);

        public async Task<IEnumerable<ActivityLog>> GetRecentAsync(int projectId, int take) =>
            await Query.Where(a => a.ProjectId == projectId)
                       .OrderByDescending(a => a.Timestamp)
                       .Take(take)
                       .ToListAsync();

        public async Task<IEnumerable<ActivityLog>> GetByProjectAsync(int projectId) =>
            await Query.Where(a => a.ProjectId == projectId)
                       .OrderByDescending(a => a.Timestamp)
                       .ToListAsync();

        public async Task<IEnumerable<ActivityLog>> GetByUserAsync(int userId) =>
            await Query.Where(a => a.UserId == userId)
                       .OrderByDescending(a => a.Timestamp)
                       .ToListAsync();

        public async Task<ActivityLog> AddAsync(ActivityLog log)
        {
            _ctx.ActivityLog.Add(log);
            await _ctx.SaveChangesAsync();
            return log;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _ctx.ActivityLog.FindAsync(id);
            if (entity is null) return false;

            _ctx.ActivityLog.Remove(entity);
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}