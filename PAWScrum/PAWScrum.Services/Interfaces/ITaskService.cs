using System.Collections.Generic;
using System.Threading.Tasks;
using PAWScrum.Models.DTOs.Tasks;

namespace PAWScrum.Services.Interfaces
{
    public interface ITaskService
    {
        Task<List<TaskResponseDto>> GetAllAsync();
        Task<TaskResponseDto> GetByIdAsync(int id);
        Task<TaskResponseDto> CreateAsync(TaskCreateDto dto);
        Task<TaskResponseDto> UpdateAsync(int id, TaskUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> AssignUserAsync(int taskId, int userId);
        Task<bool> UpdateHoursAsync(int id, decimal completedHours);
        Task<bool> ExistsAsync(int id);
    }
}
