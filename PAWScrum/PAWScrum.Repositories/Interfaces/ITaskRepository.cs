using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAWScrum.Models;
using PAWScrum.Models.Entities;

namespace PAWScrum.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<UserTask>> GetAllAsync();
        Task<UserTask> GetByIdAsync(int id);
        Task<UserTask> AddAsync(UserTask task);
        Task<UserTask> UpdateAsync(UserTask entity);
        Task<bool> DeleteAsync(int id);
        Task<UserTask> AssignUserAsync(int taskId, int userId);
        Task<UserTask> UpdateHoursAsync(int taskId, decimal hoursCompleted);
        
    }
}
