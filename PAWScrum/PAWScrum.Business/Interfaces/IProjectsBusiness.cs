using PAWScrum.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PAWScrum.Business.Interfaces
{
    public interface IProjectBusiness
    {
        Task<IEnumerable<Project>> GetAllAsync(bool includeOwner = true);
        Task<Project?> GetByIdAsync(int id, bool includeOwner = true);
        Task<bool> CreateAsync(Project project);
        Task<bool> UpdateAsync(Project project);
        Task<bool> DeleteAsync(int id);
    }
}
