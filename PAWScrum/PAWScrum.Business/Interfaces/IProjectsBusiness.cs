using PAWScrum.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PAWScrum.Business.Interfaces
{
    public interface IProjectBusiness
    {
        Task<Projects?> GetByIdAsync(int id, bool includeOwner = true);
        Task<IEnumerable<Projects>> GetAllAsync(bool includeOwner = true);
        Task<bool> CreateAsync(Projects project);
        Task<bool> UpdateAsync(Projects project);
        Task<bool> DeleteAsync(int id);
        
        
    }
}
