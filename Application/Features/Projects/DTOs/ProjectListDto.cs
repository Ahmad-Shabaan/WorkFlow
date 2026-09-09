using Domain.Enums;
namespace Application.Features.Projects.DTOs
{
    public record ProjectListDto(Guid PublicId, string ProjectName, string ProjectDescription, DateTimeOffset StartDate, DateTimeOffset EndDate, ProjectStatus ProjectStatus)
    {
    }
}
