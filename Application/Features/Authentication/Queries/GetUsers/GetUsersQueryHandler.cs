using Application.Common.Errors;
using Application.Errors;
using Application.Features.Authentication.DTOs;
using Application.Interfaces.Common;
using Application.Interfaces.Persistence;
using MediatR;
using OneOf;

namespace Application.Features.Authentication.Queries.GetUsers
{
    public class GetUsersQueryHandler(IIdentityService identityService,ICurrentUserService currentUserService) : IRequestHandler<GetUsersQuery, OneOf<List<UserDto>,NotFoundError>>
    {
        public async Task<OneOf<List<UserDto>, NotFoundError>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var adminId = currentUserService.GetCurrentUserId();
            if (adminId is null)
                return AuthenticationErrors.UserNotFound;
            return await identityService.GetAllUsers(adminId.Value);
        }
    }
}
