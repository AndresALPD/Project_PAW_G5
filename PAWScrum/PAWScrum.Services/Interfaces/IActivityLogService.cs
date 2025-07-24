using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAWScrum.Models.DTOs.ActivityLog;

namespace PAWScrum.Services.Interfaces
{
    public interface IActivityLogService
    {
        Task<IEnumerable<ActivityLogResponseDto>> GetByProjectAsync(int projectId);
        Task<IEnumerable<ActivityLogResponseDto>> GetByUserAsync(int userId);
        Task<ActivityLogResponseDto> CreateAsync(ActivityLogCreateDto dto);
    }
}
