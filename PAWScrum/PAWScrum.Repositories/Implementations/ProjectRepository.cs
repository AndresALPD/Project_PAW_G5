using PAWScrum.Models;
using PAWScrum.Repositories.Interfaces;
using PAWScrum.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace PAWScrum.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly PAWScrumDbContext _context;

        public ProjectRepository(PAWScrumDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Project>> GetAllAsync(bool includeOwner = true)
        {
            var query = _context.Projects
                .Where(p => !p.IsArchived)
                .AsNoTracking();

            if (includeOwner)
            {
                query = query.Include(p => p.Owner);
            }

            return await query.ToListAsync();
        }

        public async Task<Project?> GetByIdAsync(int id, bool includeOwner = true)
        {
            var query = _context.Projects.AsQueryable();

            if (includeOwner)
            {
                query = query.Include(p => p.Owner);
            }

            return await query
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProjectId == id);
        }

        public async Task<bool> CreateAsync(Project project)
        {
            try
            {
                await _context.Projects.AddAsync(project);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (DbUpdateException ex)
            {
                throw new ApplicationException("Error creating project.", ex);
            }
        }

        public async Task<bool> UpdateAsync(Project project)
        {
            try
            {
                _context.Entry(project).State = EntityState.Modified;
                return await _context.SaveChangesAsync() > 0;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ApplicationException("Concurrency error updating project.", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var project = await _context.Projects.FindAsync(id);
                if (project == null) return false;

                _context.Projects.Remove(project);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (DbUpdateException ex)
            {
                throw new ApplicationException("Error deleting project.", ex);
            }
        }
    }
}