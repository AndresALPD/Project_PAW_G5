using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAWScrum.Models;
using PAWScrum.Models.DTOs.Sprints;

namespace PAWScrum.Services.Interfaces
{
    public interface ISprintService
    {
        Task<IEnumerable<SprintDto>> GetAllAsync();
        Task<SprintDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(SprintCreateDto sprint);
        Task<bool> UpdateAsync(int id, SprintCreateDto sprint);
        Task<bool> DeleteAsync(int id);
    }
}

