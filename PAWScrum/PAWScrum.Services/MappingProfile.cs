using AutoMapper;
using PAWScrum.Models;
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

        }
    }
}
