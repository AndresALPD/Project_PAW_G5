using PAWScrum.Models;
using PAWScrum.Repositories;
using PAWScrum.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PAWScrum.Repositories.Implementations;

namespace PAWScrum.Repositories.Interfaces
{
    public class ProductBacklogRepository : IProductBacklogRepository
    {
        private readonly PAWScrumDbContext _context;

        public ProductBacklogRepository(PAWScrumDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductBacklogItem>> GetAllAsync()
        {
            return await _context.ProductBacklogItems.Include(p => p.Project).ToListAsync();
        }

        public async Task<ProductBacklogItem?> GetByIdAsync(int id)
        {
            return await _context.ProductBacklogItems.FindAsync(id);
        }

        public async Task<IEnumerable<ProductBacklogItem>> GetByProjectIdAsync(int projectId)
        {
            return await _context.ProductBacklogItems
                .Where(p => p.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task AddAsync(ProductBacklogItem item)
        {
            _context.ProductBacklogItems.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProductBacklogItem item)
        {
            _context.ProductBacklogItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.ProductBacklogItems.FindAsync(id);
            if (item != null)
            {
                _context.ProductBacklogItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}