
using Application.Features.Comments.Commands.CreateComment;
using Application.Features.Comments.Commands.UpdateComment;
using Application.Features.Comments.Queries.GetAllForTask;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CommentsController(IMediator mediatR) : BaseController
    {
        [Authorize]
        [HttpGet("{taskId:guid}")]
        public async Task<ActionResult> GetAllForTask([FromRoute] Guid taskId)
        {
            var result = await mediatR.Send(new GetAllForTaskQuery(taskId));
            return result.Match<ActionResult>(data => Ok(data), notFoundMsg => NotFound(notFoundMsg));
        }
        [Authorize(Roles = ("Manager,Employee"))]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateCommentCommand command)
        {
            var result = await mediatR.Send(command);
            return result.Match<ActionResult>(_ => NoContent(), notFoundMsg => NotFound(notFoundMsg), ConflictMsg => Conflict(ConflictMsg), _ => Forbid());
        }

        [Authorize(Roles = ("Manager,Employee"))]
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdateCommentCommand command)
        {
            var result = await mediatR.Send(command);
            return result.Match<ActionResult>(_ => NoContent(), notFoundMsg => NotFound(notFoundMsg), _ => Unauthorized(), _ => Forbid());
        }
    }
}
