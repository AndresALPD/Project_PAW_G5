using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAWScrum.Models.DTOs;

namespace PAWScrum.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskResponseDto>> GetAllAsync();
        Task<TaskResponseDto> GetByIdAsync(int id);
        Task<TaskResponseDto> CreateAsync(TaskCreateDto dto);
        Task<TaskResponseDto> UpdateAsync(int id, TaskUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
