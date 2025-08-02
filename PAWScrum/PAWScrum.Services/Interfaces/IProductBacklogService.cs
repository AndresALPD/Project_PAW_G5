using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAWScrum.Models;

namespace PAWScrum.Services.Interfaces
{
    public interface IProductBacklogService
    {
        Task<IEnumerable<ProductBacklogItem>> GetAllAsync();
        Task<ProductBacklogItem?> GetByIdAsync(int id);
        Task<IEnumerable<ProductBacklogItem>> GetByProjectIdAsync(int projectId);
        Task<bool> CreateAsync(ProductBacklogItem item);
        Task<bool> UpdateAsync(ProductBacklogItem item);
        Task<bool> DeleteAsync(int id);
    }
}

