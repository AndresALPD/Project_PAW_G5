using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PAWScrum.Models.DTOs;
using PAWScrum.Models.Entities;
using PAWScrum.Repositories.Interfaces;
using PAWScrum.Services.Interfaces;

namespace PAWScrum.Services.Service
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;
        private readonly IMapper _mapper;

        public TaskService(ITaskRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskResponseDto>> GetAllAsync()
        {
            var tasks = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TaskResponseDto>>(tasks);
        }

        public async Task<TaskResponseDto> GetByIdAsync(int id)
        {
            var task = await _repository.GetByIdAsync(id);
            return _mapper.Map<TaskResponseDto>(task);
        }

        public async Task<TaskResponseDto> CreateAsync(TaskCreateDto dto)
        {
            var task = _mapper.Map<WorkTask>(dto);
            task.CreatedAt = DateTime.UtcNow;
            await _repository.AddAsync(task);
            return _mapper.Map<TaskResponseDto>(task);
        }

        public async Task<TaskResponseDto> UpdateAsync(int id, TaskUpdateDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return null;

            _mapper.Map(dto, existing);
            existing.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(existing);
            return _mapper.Map<TaskResponseDto>(existing);
        }

        public async Task<bool> DeleteAsync(int id) => await _repository.DeleteAsync(id);
        public async Task<TaskResponseDto> AssignUserAsync(int taskId, int userId)
        {
            var task = await _repository.AssignUserAsync(taskId, userId);
            return _mapper.Map<TaskResponseDto>(task);
        }

    }


}
