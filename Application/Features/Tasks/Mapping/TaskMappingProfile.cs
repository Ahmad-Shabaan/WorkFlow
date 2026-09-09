using Application.Features.Tasks.DTOs;
using AutoMapper;
using ProjectTask = Domain.Entities.ProjectAggregate.Task;

namespace Application.Features.Tasks.Mapping
{
    public class TaskMappingProfile : Profile
    {
        public TaskMappingProfile()
        {
            CreateMap<ProjectTask, TaskDto>()
            .ForMember(dest => dest.TaskStatus, opt => opt.MapFrom(src => src.TaskStatus.ToString()));
        }
    }
}
