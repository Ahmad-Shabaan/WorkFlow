using Application.Features.Tasks.Commands.AssignToTask;
using Application.Features.Tasks.Commands.CompleteTask;
using Application.Features.Tasks.Commands.CreateTask;
using Application.Features.Tasks.Commands.DeleteTask;
using Application.Features.Tasks.Commands.UpdateTask;
using Application.Features.Tasks.Queries.GetTaskById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers
{
    public class TasksController(IMediator mediatR) : BaseController
    {
        [Authorize]
        [HttpGet("{id:guid}", Name = "GetTask")]
        public async Task<ActionResult> Get([FromRoute] Guid id)
        {
            var result = await mediatR.Send(new GetTaskByIdQuery(id));
            return result.Match<ActionResult>(taskDto => Ok(taskDto), notFoundMsg => NotFound(notFoundMsg));

        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateTaskCommand command)
        {
            var result = await mediatR.Send(command);
            return result.Match<ActionResult>(id => CreatedAtRoute("GetTask", new
            {
                Id = id
            }, null), error => NotFound(error), _ => Conflict()
            );
        }

        [Authorize(Roles = "Manager")]
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdateTaskCommand command)
        {
            var result = await mediatR.Send(command);
            return result.Match<ActionResult>(_ => NoContent(), notFoundError => NotFound(notFoundError), _ => Unauthorized(), _ => Forbid());
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete("{projectId:guid}/delete/{taskId:guid}")]
        public async Task<ActionResult> Delete([FromRoute] Guid projectId, [FromRoute] Guid taskId)
        {
            var result = await mediatR.Send(new DeleteTaskCommand(projectId, taskId));
            return result.Match<ActionResult>(_ => NoContent(), notFoundError => NotFound(notFoundError));

        }

        [Authorize]
        [HttpPatch("{projectId:guid}/complete/{taskId:guid}")]
        public async Task<ActionResult> Complete([FromRoute] Guid projectId, [FromRoute] Guid taskId)
        {
            var result = await mediatR.Send(new CompleteTaskCommand(projectId, taskId));
            return result.Match<ActionResult>(_ => NoContent(), notFoundError => NotFound(notFoundError));
        }

        [Authorize(Roles = "Manager")]
        [HttpPatch("assign-employee")]
        public async Task<ActionResult> AssignEmployee(AssignToTaskCommand command)
        {
            var result = await mediatR.Send(command);
            return result.Match<ActionResult>(_ => NoContent(), _ => Unauthorized(), notFoundError => NotFound(notFoundError), _ => Forbid(), conflictError => Conflict(conflictError));
        }

    }
}
