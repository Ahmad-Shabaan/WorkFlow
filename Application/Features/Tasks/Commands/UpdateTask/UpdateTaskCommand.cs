using Application.Common.Errors;
using MediatR;
using OneOf;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Tasks.Commands.UpdateTask
{
    public sealed record UpdateTaskCommand(Guid ProjectId, Guid TaskId , string TaskName, string TaskDescription, DateTimeOffset StartDate, DateTimeOffset EndDate, TaskStatus Status) : IRequest<OneOf<Success, NotFoundError>>
    {

    }
}
