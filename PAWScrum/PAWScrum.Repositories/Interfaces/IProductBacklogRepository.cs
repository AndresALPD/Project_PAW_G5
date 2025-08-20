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
    public interface IProductBacklogRepository
    {
        Task<IEnumerable<ProductBacklogItem>> GetAllAsync();
        Task<ProductBacklogItem?> GetByIdAsync(int id);
        Task<IEnumerable<ProductBacklogItem>> GetByProjectIdAsync(int projectId);
        Task AddAsync(ProductBacklogItem item);
        Task UpdateAsync(ProductBacklogItem item);
        Task DeleteAsync(int id);
    }
}