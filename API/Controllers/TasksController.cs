using Application.Common.Errors;
using Application.Features.Tasks.Commands.CompleteTask;
using Application.Features.Tasks.Commands.CreateTask;
using Application.Features.Tasks.Commands.DeleteTask;
using Application.Features.Tasks.Commands.UpdateTask;
using Application.Features.Tasks.Queries.GetTaskById;
using Application.Features.Tasks.Queries.GetTasks;
using BookHavenAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers
{
    public class TasksController(IMediator mediatR) : BaseController
    {

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> Get([FromRoute] Guid id)
        {
            var result = await mediatR.Send(new GetTaskByIdQuery(id));
            return result.Match<ActionResult>(taskDto => Ok(taskDto), error => error switch
            {
                NotFoundError e => NotFound(e),
                _ => StatusCode(500)
            });

        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var result = await mediatR.Send(new GetTasksQuery());
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateTaskCommand command)
        {
            var result = await mediatR.Send(command);
            return result.Match<ActionResult>(id => CreatedAtRoute("GetProjectByPublicId", new
            {
                PublicId = command.ProjectId
            }, null), error => error switch
            {
                Application.Common.Errors.NotFoundError e => NotFound(e),
                _ => StatusCode(500)
            });
        }


        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdateTaskCommand command)
        {
            var result = await mediatR.Send(command);
            return result.Match<ActionResult>(_ => NoContent(), error => error switch
            {
                NotFoundError e => NotFound(e),
                _ => StatusCode(500)
            });
        }

        [HttpDelete("{projectId:guid}/delete/{taskId:guid}")]
        public async Task<ActionResult> Delete([FromRoute] Guid projectId, [FromRoute] Guid taskId)
        {
            var result = await mediatR.Send(new DeleteTaskCommand(projectId, taskId));
            return result.Match<ActionResult>(_ => NoContent(), error => error switch
            {
                NotFoundError e => NotFound(e),
                _ => StatusCode(500)
            });
        }


        [HttpPatch("{projectId:guid}/complete/{taskId:guid}")]
        public async Task<ActionResult> Complete([FromRoute] Guid projectId, [FromRoute] Guid taskId)
        {
            var result = await mediatR.Send(new CompleteTaskCommand(projectId, taskId));
            return result.Match<ActionResult>(_ => NoContent(), error => error switch
            {
                NotFoundError e => NotFound(e),
                _ => StatusCode(500)
            });
        }


    }
}
