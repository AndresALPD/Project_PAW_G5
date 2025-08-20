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
    public class SprintRepository : ISprintRepository
    {
        private readonly PAWScrumDbContext _context;

        public SprintRepository(PAWScrumDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Sprints>> GetAllAsync()
        {
            return await _context.Sprints
                                 .Include(s => s.Project)
                                 .ToListAsync();
        }

        public async Task<Sprints?> GetByIdAsync(int id)
        {
            return await _context.Sprints
                                 .Include(s => s.Project)
                                 .FirstOrDefaultAsync(s => s.SprintId == id);
        }

        public async Task<bool> CreateAsync(Sprints sprint)
        {
            _context.Sprints.Add(sprint);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Sprints sprint)
        {
            _context.Sprints.Update(sprint);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sprint = await _context.Sprints.FindAsync(id);
            if (sprint == null) return false;

            _context.Sprints.Remove(sprint);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
