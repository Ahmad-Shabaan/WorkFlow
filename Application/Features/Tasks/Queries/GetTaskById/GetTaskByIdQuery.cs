using Application.Common.Errors;
using Application.Features.Tasks.DTOs;
using MediatR;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Tasks.Queries.GetTaskById
{
    public  sealed record GetTaskByIdQuery(Guid TaskPublicId) : IRequest<OneOf<TaskDto, NotFoundError>>
    {
    }
}
