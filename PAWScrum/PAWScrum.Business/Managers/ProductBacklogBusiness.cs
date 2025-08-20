using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAWScrum.Business.Interfaces;
using PAWScrum.Models;
using PAWScrum.Repositories.Interfaces;
using PAWScrum.Architecture.Helpers;
using PAWScrum.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using PAWScrum.Models.DTOs;
using PAWScrum.Data.Context;

namespace PAWScrum.Business.Managers
{
    public class ProductBacklogBusiness : IProductBacklogBusiness
    {
        private readonly IProductBacklogRepository _repository;

        public ProductBacklogBusiness(IProductBacklogRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductBacklogItem>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ProductBacklogItem?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ProductBacklogItem>> GetByProjectIdAsync(int projectId)
        {
            return await _repository.GetByProjectIdAsync(projectId);
        }

        public async Task AddAsync(ProductBacklogItem item)
        {
            item.CreatedAt = DateTime.Now;
            item.Status = item.Status ?? "To Do"; // esto debería ser por default
            await _repository.AddAsync(item);
        }

        public async Task UpdateAsync(ProductBacklogItem item)
        {
            await _repository.UpdateAsync(item);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
