using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAWScrum.Models.Entities;

namespace PAWScrum.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<WorkTask>> GetAllAsync();
        Task<WorkTask> GetByIdAsync(int id);
        Task<WorkTask> AddAsync(WorkTask task);
        Task<WorkTask> UpdateAsync(WorkTask task);
        Task<bool> DeleteAsync(int id);
        Task<WorkTask> AssignUserAsync(int taskId, int userId);

    }
}
