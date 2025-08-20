using PAWScrum.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PAWScrum.Repositories.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllAsync(bool includeOwner = true);
        Task<Project?> GetByIdAsync(int id, bool includeOwner = true);
        Task<bool> CreateAsync(Project project);
        Task<bool> UpdateAsync(Project project);
        Task<bool> DeleteAsync(int id);
    }
}