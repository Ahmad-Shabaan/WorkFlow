using Application.Interfaces.Persistence;
using Domain.Entities.ProjectAggregate;
using MediatR;
using OneOf;

namespace Application.Features.Projects.Commands.CreateProject
{
    public class CreateProjectCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateProjectCommand, OneOf<Guid>>
    {
        public async Task<OneOf<Guid>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = new Project(request.ProjectName, request.ProjectDescription, request.StartDate, request.EndDate);
            unitOfWork.Repository<Project>().Add(project);
            await unitOfWork.Complete(cancellationToken);
            return project.PublicId;
        }
    }
}
