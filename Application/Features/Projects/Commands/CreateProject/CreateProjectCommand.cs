using Domain.Entities.ProjectAggregate;
using MediatR;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Projects.Commands.CreateProject
{
    public sealed record CreateProjectCommand(string ProjectName, string ProjectDescription, DateTimeOffset StartDate, DateTimeOffset EndDate) : IRequest<OneOf<Guid>>
    {
    }
}
