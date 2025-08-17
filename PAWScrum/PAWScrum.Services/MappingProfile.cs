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
                .ForMember(d => d.HoursEstimated, o => o.MapFrom(s => s.EstimationHours))
                .ForMember(d => d.HoursCompleted, o => o.MapFrom(s => s.CompletedHours))
                .ForMember(d => d.AssignedUserId, o => o.MapFrom(s => s.AssignedTo))
                .ForMember(d => d.AssignedUserName, o => o.MapFrom(s => s.AssignedToNavigation != null ? s.AssignedToNavigation.Username : null))
                .ForMember(d => d.ProductBacklogItemId, o => o.MapFrom(s => s.SprintItemId))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status ?? "To Do")); // <- coalesce

            CreateMap<TaskCreateDto, UserTask>()
                .ForMember(d => d.TaskId, opt => opt.Ignore())
                .ForMember(d => d.SprintItemId, opt => opt.MapFrom(s => s.ProductBacklogItemId))
                .ForMember(d => d.EstimationHours, opt => opt.MapFrom(s => (decimal?)s.HoursEstimated))
                .ForMember(d => d.CompletedHours, opt => opt.MapFrom(_ => 0m))
                .ForMember(d => d.Status, opt => opt.MapFrom(_ => "To Do"));

            CreateMap<TaskUpdateDto, UserTask>()
                .ForMember(d => d.TaskId, opt => opt.Ignore())
                .ForMember(d => d.SprintItemId, opt => opt.MapFrom(s => s.ProductBacklogItemId))
                .ForMember(d => d.EstimationHours, opt => opt.MapFrom(s => (decimal?)s.HoursEstimated))
                .ForMember(d => d.CompletedHours, opt => opt.MapFrom(s => (decimal?)s.HoursCompleted))
                .ForMember(d => d.AssignedTo, opt => opt.MapFrom(s => s.AssignedUserId));

            CreateMap<Comment, CommentResponseDto>()
                .ForMember(d => d.TaskId, o => o.MapFrom(s => s.TaskId))
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.User != null ? s.User.Username : null));
       

            CreateMap<CommentCreateDto, Comment>()
                .ForMember(d => d.TaskId, o => o.MapFrom(s => s.TaskId)) 
                .ForMember(d => d.CreatedAt, o => o.MapFrom(_ => DateTime.UtcNow));

        }
    }
}
