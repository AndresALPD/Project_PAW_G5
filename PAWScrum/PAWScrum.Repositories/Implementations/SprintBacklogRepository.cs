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
    public class SprintBacklogRepository : ISprintBacklogRepository
    {
        private readonly PAWScrumDbContext _context;

        public SprintBacklogRepository(PAWScrumDbContext context)
        {
            _context = context;
        }

        public async Task<List<SprintBacklogItem>> GetAllAsync()
        {
            return await _context.SprintBacklogItems
                .AsNoTracking()
                .Select(s => new SprintBacklogItem
                {
                    SprintItemId = s.SprintItemId,
                    SprintId = s.SprintId,
                    ProductBacklogItemId = s.ProductBacklogItemId,
                    AssignedTo = s.AssignedTo,
                    Status = s.Status,
                    EstimationHours = s.EstimationHours,
                    CompletedHours = s.CompletedHours
                })
                .ToListAsync();
        }

        public async Task<SprintBacklogItem?> GetByIdAsync(int id)
        {
            return await _context.SprintBacklogItems
                .AsNoTracking()
                .Where(s => s.SprintItemId == id)
                .Select(s => new SprintBacklogItem
                {
                    SprintItemId = s.SprintItemId,
                    SprintId = s.SprintId,
                    ProductBacklogItemId = s.ProductBacklogItemId,
                    AssignedTo = s.AssignedTo,
                    Status = s.Status,
                    EstimationHours = s.EstimationHours,
                    CompletedHours = s.CompletedHours
                })
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(SprintBacklogItem item)
        {
            await _context.SprintBacklogItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SprintBacklogItem item)
        {
            _context.SprintBacklogItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.SprintBacklogItems.FindAsync(id);
            if (item != null)
            {
                _context.SprintBacklogItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<SprintBacklogItem>> GetBySprintIdAsync(int sprintId)
        {
            return await _context.SprintBacklogItems
                .AsNoTracking()
                .Where(x => x.SprintId == sprintId)
                .Select(s => new SprintBacklogItem
                {
                    SprintItemId = s.SprintItemId,
                    SprintId = s.SprintId,
                    ProductBacklogItemId = s.ProductBacklogItemId,
                    AssignedTo = s.AssignedTo,
                    Status = s.Status,
                    EstimationHours = s.EstimationHours,
                    CompletedHours = s.CompletedHours
                })
                .ToListAsync();
        }
    }
}
