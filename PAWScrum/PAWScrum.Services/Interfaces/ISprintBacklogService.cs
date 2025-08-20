using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAWScrum.Models;
using PAWScrum.Models.DTOs.SprintBacklog;

namespace PAWScrum.Services.Interfaces
{
    public interface ISprintBacklogService
    {
        Task<IEnumerable<SprintBacklogDto>> GetAllAsync();
        Task<SprintBacklogDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(SprintBacklogCreateDto dto);
        Task<bool> UpdateAsync(int id, SprintBacklogCreateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}

