using Application.Common.Errors;
using Application.Errors;
using Application.Features.Comments.DTOs;
using Application.Features.Comments.Specifications;
using Application.Interfaces.Persistence;
using MediatR;
using OneOf;

namespace Application.Features.Comments.Queries.GetAllForTask
{
    public class GetAllForTaskQueryHandler(IIdentityService identityService, ITaskRepository taskRepository) : IRequestHandler<GetAllForTaskQuery, OneOf<IReadOnlyList<CommentResponseDto>, NotFoundError>>
    {
        public async Task<OneOf<IReadOnlyList<CommentResponseDto>, NotFoundError>> Handle(GetAllForTaskQuery request, CancellationToken cancellationToken)
        {
            var taskSpec = new GetTaskByPublicIdWithComments(request.TaskId);
            var task = await taskRepository.Get(taskSpec, cancellationToken);
            if (task is null)
                return TaskErrors.NotFoundError;
            var authorsIdx = task.Comments.Select(c => c.AuthorId).Distinct().ToList();
            var authors = await identityService.GetTargetUsersName(authorsIdx);
            var authorsById = authors.ToDictionary(a => a.Id); // to inhance performance of search

            var comments = task.Comments.Select(c => new CommentResponseDto(c.PublicId, authorsById[c.AuthorId].FullName, c.Content)).ToList();

            return comments;
        }
    }
}
