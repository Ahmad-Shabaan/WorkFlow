using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommandValidator : AbstractValidator<DeleteTaskCommand>
    {
        public DeleteTaskCommandValidator()
        {

            RuleFor(x => x.TaskId)
                .NotEmpty().WithMessage("Task Id is required.");

            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("Project Id is required.");
        }
    }
}
