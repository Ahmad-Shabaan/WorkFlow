using Application.Common.Errors;
using Application.Features.Authentication.DTOs;
using MediatR;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Authentication.Queries.GetUsers
{
    public sealed record GetUsersQuery : IRequest<OneOf<List<UserDto>, NotFoundError>>
    {
    }
}
