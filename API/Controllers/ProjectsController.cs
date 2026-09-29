using Application.Common.Errors;
using Application.Features.Projects.Commands.CancelProject;
using Application.Features.Projects.Commands.CompleteProject;
using Application.Features.Projects.Commands.CreateProject;
using Application.Features.Projects.Commands.DeleteProject;
using Application.Features.Projects.Commands.StartProject;
using Application.Features.Projects.Commands.UpdateProject;
using Application.Features.Projects.Queries.GetAllProjects;
using Application.Features.Projects.Queries.GetProject;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProjectsController(IMediator mediator) : BaseController
    {
        [Authorize]
        [HttpGet("{id:guid}", Name = "GetProjectByPublicId")]
        public async Task<ActionResult> Get([FromRoute] Guid id)
        {
            var result = await mediator.Send(new GetProjectQuery(id));
            return result.Match<ActionResult>(projectDto => Ok(projectDto), error => error switch
            {
                NotFoundError e => NotFound(e),
                _ => StatusCode(500)
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var result = await mediator.Send(new GetAllProjectsQuery());
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateProjectCommand command)
        {
            var result = await mediator.Send(command);
            return result.Match<ActionResult>(id => CreatedAtRoute("GetProjectByPublicId", new { Id = id }, null),
                notFoundError => NotFound(notFoundError), conflictError => Conflict(conflictError), _ => Unauthorized());
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdateProjectCommand command)
        {
            var result = await mediator.Send(command);
            return result.Match<ActionResult>(_ => NoContent(),
                notFoundError => NotFound(notFoundError), _ => Forbid(), _ => Unauthorized(), conflictMsg => Conflict(conflictMsg));
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            var result = await mediator.Send(new DeleteProjectCommand(id));
            return result.Match<ActionResult>(_ => NoContent(), notFoundError => NotFound(notFoundError), _ => Forbid());
        }


        [Authorize(Roles = "Manager")]
        [HttpPatch("start/{id:guid}")]
        public async Task<ActionResult> Start([FromRoute] Guid id)
        {
            var result = await mediator.Send(new StartProjectCommand(id));
            return result.Match<ActionResult>(_ => NoContent(), notFoundError => NotFound(notFoundError), _ => Forbid());

        }
        [Authorize(Roles = "Manager")]
        [HttpPatch("cancel/{id:guid}")]
        public async Task<ActionResult> Cancel([FromRoute] Guid id)
        {
            var result = await mediator.Send(new CancelProjectCommand(id));
            return result.Match<ActionResult>(_ => NoContent(), notFoundError => NotFound(notFoundError));
        }
        [Authorize(Roles = "Manager")]
        [HttpPatch("complete/{id:guid}")]
        public async Task<ActionResult> Complete([FromRoute] Guid id)
        {
            var result = await mediator.Send(new CompleteProjectCommand(id));
            return result.Match<ActionResult>(_ => NoContent(), notFoundError => NotFound(notFoundError));
        }

    }
}
