using Application.Features.Tasks.DTOs;

namespace Application.Features.Projects.DTOs
{
    public record ProjectResponseDto(Guid ManagerId, string Manager, Guid PublicId, string ProjectName, string ProjectDescription, DateTimeOffset StartDate, DateTimeOffset EndDate, string ProjectStatus, IReadOnlyList<TaskDto> Tasks)
    {
    }
}
