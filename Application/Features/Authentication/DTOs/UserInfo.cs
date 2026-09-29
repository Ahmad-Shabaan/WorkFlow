using Domain.Entities;

namespace Application.Features.Authentication.DTOs
{
    public record UserInfo(int Id,Guid PublicId, string DisplayName, string Email, Project? Project, IReadOnlyCollection<string> Roles )
    {
    }
}
