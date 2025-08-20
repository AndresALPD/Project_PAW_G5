using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PAWScrum.Models;
using PAWScrum.Models.DTOs.ActivityLog;
using PAWScrum.Models.Entities;
using PAWScrum.Repositories.Interfaces;
using PAWScrum.Services.Interfaces;

namespace PAWScrum.Services.Service
{
    public class ActivityLogService : IActivityLogService
    {
        private readonly IActivityLogRepository _repo;
        public ActivityLogService(IActivityLogRepository repo) => _repo = repo;

        public Task<IEnumerable<ActivityLog>> GetRecentAsync(int projectId, int take = 20)
            => _repo.GetRecentAsync(projectId, take);

        public Task<IEnumerable<ActivityLog>> GetByProjectAsync(int projectId)
            => _repo.GetByProjectAsync(projectId);

        public Task<IEnumerable<ActivityLog>> GetByUserAsync(int userId)
            => _repo.GetByUserAsync(userId);

        public Task<ActivityLog?> GetByIdAsync(int id)
            => _repo.GetByIdAsync(id);

        // <- devuelve el objeto creado
        public Task<ActivityLog> CreateAsync(ActivityLog log)
            => _repo.AddAsync(log);

        // <- devuelve true/false
        public Task<bool> DeleteAsync(int id)
            => _repo.DeleteAsync(id);
    }
}