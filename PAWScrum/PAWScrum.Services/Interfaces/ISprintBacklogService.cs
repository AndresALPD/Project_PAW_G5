using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAWScrum.Models;

namespace PAWScrum.Services.Interfaces
{
    public interface ISprintBacklogService
    {
        Task<IEnumerable<SprintBacklogItem>> GetAllAsync();
        Task<SprintBacklogItem?> GetByIdAsync(int id);
        Task<IEnumerable<SprintBacklogItem>> GetBySprintIdAsync(int sprintId);
        Task<bool> CreateAsync(SprintBacklogItem item);
        Task<bool> UpdateAsync(SprintBacklogItem item);
        Task<bool> DeleteAsync(int id);
    }
}

