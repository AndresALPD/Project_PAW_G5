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

        public Task<ActivityLog?> GetByIdAsync(int id) =>
            _ctx.ActivityLog
                .AsNoTracking()
                .Include(a => a.User)
                .Include(a => a.Project)
                .FirstOrDefaultAsync(a => a.ActivityId == id);

        public Task<IEnumerable<ActivityLog>> GetRecentAsync(int projectId, int take = 20) =>
            _ctx.ActivityLog
                .AsNoTracking()
                .Where(a => a.ProjectId == projectId)
                .Include(a => a.User)       
                .Include(a => a.Project)    
                .OrderByDescending(a => a.Timestamp)
                .Take(take)
                .ToListAsync()
                .ContinueWith(t => (IEnumerable<ActivityLog>)t.Result);

        public Task<IEnumerable<ActivityLog>> GetByProjectAsync(int projectId) =>
            _ctx.ActivityLog
                .AsNoTracking()
                .Where(a => a.ProjectId == projectId)
                .Include(a => a.User)
                .Include(a => a.Project)
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync()
                .ContinueWith(t => (IEnumerable<ActivityLog>)t.Result);

        public Task<IEnumerable<ActivityLog>> GetByUserAsync(int userId) =>
            _ctx.ActivityLog
                .AsNoTracking()
                .Where(a => a.UserId == userId)
                .Include(a => a.User)
                .Include(a => a.Project)
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync()
                .ContinueWith(t => (IEnumerable<ActivityLog>)t.Result);

        public async Task<ActivityLog> AddAsync(ActivityLog log)
        {
            _ctx.ActivityLog.Add(log);
            await _ctx.SaveChangesAsync();
            return log;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _ctx.ActivityLog.FindAsync(id);
            if (entity == null) return false;
            _ctx.ActivityLog.Remove(entity);
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}