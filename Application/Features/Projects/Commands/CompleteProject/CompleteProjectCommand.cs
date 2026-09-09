using Application.Common.Errors;
using MediatR;
using OneOf;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Projects.Commands.CompleteProject
{
    public sealed record CompleteProjectCommand(Guid ProjectId) : IRequest<OneOf<Success, NotFoundError>>
    {
    }
}
