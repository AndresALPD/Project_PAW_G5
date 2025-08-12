using AutoMapper;
using PAWScrum.Models;
using PAWScrum.Models.DTOs.ActivityLog;
using PAWScrum.Models.DTOs.Comments;
using PAWScrum.Models.DTOs.Tasks;
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


            CreateMap<Comment, CommentResponseDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username));
            CreateMap<CommentCreateDto, Comment>();

            CreateMap<ActivityLog, ActivityLogResponseDto>();

            CreateMap<ActivityLog,ActivityLogResponseDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.ProjectName));
            CreateMap<ActivityLogCreateDto, ActivityLog>();

            CreateMap<ActivityLog, ActivityLogResponseDto>()
            .ForMember(d => d.LogId, opt => opt.MapFrom(s => s.LogId)); 
                                                             
            CreateMap<ActivityLog, ActivityLogResponseDto>();

        }
    }
}
