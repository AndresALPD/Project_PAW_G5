using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAWScrum.Models.DTOs.ActivityLog;
using PAWScrum.Models.Entities;

namespace PAWScrum.Services.Interfaces
{
    public interface IActivityLogService
    {
        Task<IEnumerable<ActivityLog>> GetRecentAsync(int projectId, int take = 20);
        Task<IEnumerable<ActivityLog>> GetByProjectAsync(int projectId);
        Task<IEnumerable<ActivityLog>> GetByUserAsync(int userId);
        Task<ActivityLog?> GetByIdAsync(int id);
        Task<ActivityLog> CreateAsync(ActivityLog log);
        Task<bool> DeleteAsync(int id);
    }
}
