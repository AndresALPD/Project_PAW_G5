using System;
using AutoMapper;
using PAWScrum.Models;                      
using PAWScrum.Models.DTOs.Tasks;           
using PAWScrum.Models.DTOs.Comments;       
using PAWScrum.Models.DTOs.ActivityLog;
using PAWScrum.Models.Entities;
namespace PAWScrum.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserTask, TaskResponseDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.TaskId))
                .ForMember(d => d.AssignedUserId, o => o.MapFrom(s => s.AssignedTo))
                .ForMember(d => d.EstimationHours, o => o.MapFrom(s => s.EstimationHours))
                .ForMember(d => d.CompletedHours, o => o.MapFrom(s => s.CompletedHours));

            CreateMap<TaskCreateDto, UserTask>()
                .ForMember(d => d.AssignedTo, o => o.MapFrom(s => s.AssignedUserId));

            CreateMap<TaskUpdateDto, UserTask>()
                .ForMember(d => d.AssignedTo, o => o.MapFrom(s => s.AssignedUserId));

            CreateMap<Comment, CommentResponseDto>()
                .ForMember(d => d.TaskId, o => o.MapFrom(s => s.TaskId))
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.User != null ? s.User.Username : null));
       

            CreateMap<CommentCreateDto, Comment>()
                .ForMember(d => d.TaskId, o => o.MapFrom(s => s.TaskId)) 
                .ForMember(d => d.CreatedAt, o => o.MapFrom(_ => DateTime.UtcNow));

        }
    }
}
