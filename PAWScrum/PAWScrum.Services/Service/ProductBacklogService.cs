using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using PAWScrum.Business.Interfaces;
using PAWScrum.Models;
using PAWScrum.Services.Interfaces;

namespace PAWScrum.Services.Service
{
    public class ProductBacklogService : IProductBacklogService
    {
        private readonly IProductBacklogBusiness _productBacklogBusiness;

        public ProductBacklogService(IProductBacklogBusiness productBacklogBusiness)
        {
            _productBacklogBusiness = productBacklogBusiness;
        }

        public async Task<IEnumerable<ProductBacklogItem>> GetAllAsync()
        {
            return await _productBacklogBusiness.GetAllAsync();
        }

        public async Task<ProductBacklogItem?> GetByIdAsync(int id)
        {
            return await _productBacklogBusiness.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ProductBacklogItem>> GetByProjectIdAsync(int projectId)
        {
            return await _productBacklogBusiness.GetByProjectIdAsync(projectId);
        }

        public async Task<bool> CreateAsync(ProductBacklogItem item)
        {
            try
            {
                await _productBacklogBusiness.AddAsync(item);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(ProductBacklogItem item)
        {
            try
            {
                await _productBacklogBusiness.UpdateAsync(item);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _productBacklogBusiness.DeleteAsync(id);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

