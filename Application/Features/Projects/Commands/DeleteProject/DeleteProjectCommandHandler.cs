using Application.Common.Errors;
using Application.Errors;
using Application.Interfaces.Persistence;
using Domain.Entities.ProjectAggregate;
using MediatR;
using OneOf;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Projects.Commands.DeleteProject
{
    public class DeleteProjectCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteProjectCommand, OneOf<Success, NotFoundError>>
    {
        public async Task<OneOf<Success, NotFoundError>> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await unitOfWork.Repository<Project>().Get(request.ProjectId, cancellationToken);
            if (project == null)
                return ProjectErrors.NotFoundError;
            unitOfWork.Repository<Project>().Delete(project, cancellationToken);
            await unitOfWork.Complete(cancellationToken);
            return new Success();
        }
    }
}
