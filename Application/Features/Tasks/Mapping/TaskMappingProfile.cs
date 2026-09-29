using Application.Features.Tasks.DTOs;
using AutoMapper;
using Task = Domain.Entities.Task;

namespace Application.Features.Tasks.Mapping
{
    public class TaskMappingProfile : Profile
    {
        public TaskMappingProfile()
        {
            CreateMap<Task, TaskDto>()
            .ForMember(dest => dest.TaskStatus, opt => opt.MapFrom(src => src.TaskStatus.ToString()));
        }
    }
}
