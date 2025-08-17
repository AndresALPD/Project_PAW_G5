using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PAWScrum.Models;
using PAWScrum.Models.DTOs.Tasks;
using PAWScrum.Models.Entities;
using PAWScrum.Repositories.Interfaces;
using PAWScrum.Services.Interfaces;

namespace PAWScrum.Services.Service
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repo;
        private readonly IMapper _mapper;

        public TaskService(ITaskRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskResponseDto>> GetAllAsync()
        {
            var items = await _repo.GetAllAsync();
            return items.Select(t => _mapper.Map<TaskResponseDto>(t));
        }

        public async Task<TaskResponseDto> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<TaskResponseDto>(entity);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity != null;
        }

        public async Task<bool> UpdateHoursAsync(int id, decimal hoursCompleted)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            entity.CompletedHours = hoursCompleted;
            await _repo.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> AssignUserAsync(int taskId, int userId)
        {
            var entity = await _repo.GetByIdAsync(taskId);
            if (entity == null) return false;

            entity.AssignedTo = userId;
            await _repo.UpdateAsync(entity);
            return true;
        }
    }
}