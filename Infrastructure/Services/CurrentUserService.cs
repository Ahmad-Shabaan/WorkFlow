using Application.Interfaces.Common;
using Infrastructure.Persistence.Extensions;
using Microsoft.AspNetCore.Http;
namespace Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public int? GetCurrentUserId()
            => _httpContextAccessor.HttpContext?.User.GetUserId();
        public string? GetCurrentUserEmail()
            => _httpContextAccessor.HttpContext?.User.GetEmail();
        public IEnumerable<string>? GetCurrentUserRoles()
            => _httpContextAccessor.HttpContext?.User.GetRoles();
    }
}
