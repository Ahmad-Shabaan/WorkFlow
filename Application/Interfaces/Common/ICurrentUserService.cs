namespace Application.Interfaces.Common
{
    public interface ICurrentUserService
    {
        string? GetCurrentUserEmail();
        int? GetCurrentUserId();
        IEnumerable<string>? GetCurrentUserRoles();
    }
}
