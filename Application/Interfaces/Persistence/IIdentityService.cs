using Application.Features.Authentication.DTOs;
using Application.Features.Comments.DTOs;
using Application.Features.Tasks.DTOs;
namespace Application.Interfaces.Persistence
{
    public interface IIdentityService
    {
        Task AssignRole(Guid userId, int roleId);
        Task<List<UserDto>> GetAllUsers(int adminId);
        Task<IEnumerable<EmployeeDto>> GetTargetUsers(List<int> ids);
        Task<IEnumerable<AuthorDto>> GetTargetUsersName(List<int> ids);
        Task<UserInfo?> GetUser(Guid userId);
        Task<int?> GetUserId(Guid userId);
        Task<EmployeeDto?> GetUserName(int id);
        Task<IEnumerable<string>> GetUserRoles(Guid userId);
        Task<LoginDto> Login(string email, string password, CancellationToken cancellationToken);
        Task Logout(string refreshToken, CancellationToken cancellationToken);
        Task<LoginDto> RefreshToken(string refreshToken, CancellationToken cancellationToken);
        Task RegisterAsync(RegisterDto registerDto);
    }
}
