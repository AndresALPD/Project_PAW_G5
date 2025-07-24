using AutoMapper;
using PAWScrum.Models.DTOs;
using PAWScrum.Models.Entities;

namespace PAWScrum.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<WorkTask, TaskResponseDto>();
            CreateMap<TaskCreateDto, WorkTask>();
            CreateMap<TaskUpdateDto, WorkTask>();

        }
    }
}
