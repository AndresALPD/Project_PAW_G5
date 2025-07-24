using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PAWScrum.Models;
using PAWScrum.Models.DTOs.ActivityLog;
using PAWScrum.Repositories.Interfaces;
using PAWScrum.Services.Interfaces;

namespace PAWScrum.Services.Service
{
    public class ActivityLogService : IActivityLogService
    {
        private readonly IActivityLogRepository _repository;
        private readonly IMapper _mapper;

        public ActivityLogService(IActivityLogRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ActivityLogResponseDto>> GetByProjectAsync(int projectId)
        {
            var logs = await _repository.GetByProjectAsync(projectId);
            return _mapper.Map<IEnumerable<ActivityLogResponseDto>>(logs);
        }

        public async Task<IEnumerable<ActivityLogResponseDto>> GetByUserAsync(int userId)
        {
            var logs = await _repository.GetByUserAsync(userId);
            return _mapper.Map<IEnumerable<ActivityLogResponseDto>>(logs);
        }

        public async Task<ActivityLogResponseDto> CreateAsync(ActivityLogCreateDto dto)
        {
            var log = _mapper.Map<ActivityLog>(dto);
            log.Timestamp = DateTime.UtcNow;
            await _repository.AddAsync(log);
            return _mapper.Map<ActivityLogResponseDto>(log);
        }
    }
}
