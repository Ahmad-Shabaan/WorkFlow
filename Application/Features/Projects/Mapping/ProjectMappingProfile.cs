using Application.Features.Projects.DTOs;
using Application.Features.Tasks.DTOs;
using AutoMapper;
using Domain.Entities.ProjectAggregate;
namespace Application.Features.Projects.Mapping
{
    public class ProjectMappingProfile : Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<Project, ProjectDto>()
                .ForMember(dest => dest.ProjectStatus, opt => opt.MapFrom(src => src.ProjectStatus.ToString()))
                .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.Tasks));
        }
    }
}
