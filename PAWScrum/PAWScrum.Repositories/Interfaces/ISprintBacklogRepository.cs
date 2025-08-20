using PAWScrum.Data.Context;
using PAWScrum.Models;
using PAWScrum.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWScrum.Repositories.Implementations
{
    public interface ISprintBacklogRepository
    {
        Task<List<SprintBacklogItem>> GetAllAsync();
        Task<SprintBacklogItem?> GetByIdAsync(int id);
        Task AddAsync(SprintBacklogItem item);
        Task UpdateAsync(SprintBacklogItem item);
        Task DeleteAsync(int id);
        Task<List<SprintBacklogItem>> GetBySprintIdAsync(int sprintId);
    }
}