using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAWScrum.Models;
using PAWScrum.Models.DTOs;

namespace PAWScrum.Business.Interfaces
{
    public interface IProductBacklogBusiness
    {
        Task<IEnumerable<ProductBacklogItem>> GetAllAsync();
        Task<ProductBacklogItem?> GetByIdAsync(int id);
        Task<IEnumerable<ProductBacklogItem>> GetByProjectIdAsync(int projectId);
        Task AddAsync(ProductBacklogItem item);
        Task UpdateAsync(ProductBacklogItem item);
        Task DeleteAsync(int id);
    }
}
