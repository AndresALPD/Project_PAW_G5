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
        private readonly IActivityLogRepository _repository;

        public ActivityLogService(IActivityLogRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<ActivityLog>> GetRecentAsync(int projectId, int take = 20)
            => _repository.GetRecentAsync(projectId, take);

        public Task<IEnumerable<ActivityLog>> GetByProjectAsync(int projectId)
            => _repository.GetByProjectAsync(projectId);

        public Task<IEnumerable<ActivityLog>> GetByUserAsync(int userId)
            => _repository.GetByUserAsync(userId);

        public Task<ActivityLog> CreateAsync(ActivityLog entity)
            => _repository.AddAsync(entity);

        public Task<bool> DeleteAsync(int id)
            => _repository.DeleteAsync(id);
    }
}
