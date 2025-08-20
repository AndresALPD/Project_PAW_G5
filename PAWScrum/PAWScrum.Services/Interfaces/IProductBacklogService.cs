using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAWScrum.Models;
using PAWScrum.Models.DTOs.ProductBacklog;

namespace PAWScrum.Services.Interfaces
{
    public interface IProductBacklogService
    {
        Task<IEnumerable<ProductBacklogDto>> GetAllAsync();
        Task<ProductBacklogDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(ProductBacklogCreateDto dto);
        Task<bool> UpdateAsync(int id, ProductBacklogCreateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}

