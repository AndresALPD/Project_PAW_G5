using PAWScrum.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PAWScrum.Repositories.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Projects>> GetAllAsync(bool includeOwner = true);
        Task<Projects?> GetByIdAsync(int id, bool includeOwner = true);
        Task<bool> CreateAsync(Projects project);
        Task<bool> UpdateAsync(Projects project);
        Task<bool> DeleteAsync(int id);
    }
}