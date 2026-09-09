namespace Application.Features.Tasks.DTOs
{
    public record TaskDto(Guid PublicId, string TaskName, string TaskDescription, DateTimeOffset StartDate, DateTimeOffset EndDate, string TaskStatus)
    {
    }
}
