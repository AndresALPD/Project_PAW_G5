using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAWScrum.Models;

namespace PAWScrum.Repositories.Interfaces
{
    public interface IActivityLogRepository
    {
        Task<IEnumerable<ActivityLog>> GetByProjectAsync(int projectId);
        Task<IEnumerable<ActivityLog>> GetByUserAsync(int userId);
        Task<ActivityLog> AddAsync(ActivityLog log);
    }
}
