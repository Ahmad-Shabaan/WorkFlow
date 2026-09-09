using Application.Features.Tasks.DTOs;
namespace Application.Features.Projects.DTOs
{
    public record ProjectDto(Guid PublicId, string ProjectName, string ProjectDescription, DateTimeOffset StartDate, DateTimeOffset EndDate, string ProjectStatus, IReadOnlyList<TaskDto> Tasks)
    {
    }
}
