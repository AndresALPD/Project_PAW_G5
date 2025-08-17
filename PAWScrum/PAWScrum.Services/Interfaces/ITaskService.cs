using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAWScrum.Models.DTOs.Tasks;

namespace PAWScrum.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskResponseDto>> GetAllAsync();
        Task<TaskResponseDto> GetByIdAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> UpdateHoursAsync(int id, decimal hoursCompleted);
        Task<bool> AssignUserAsync(int taskId, int userId);
    }
}
