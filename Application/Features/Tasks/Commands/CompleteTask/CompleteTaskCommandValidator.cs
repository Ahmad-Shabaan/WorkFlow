

using FluentValidation;

namespace Application.Features.Tasks.Commands.CompleteTask
{
    public class CompleteTaskCommandValidator : AbstractValidator<CompleteTaskCommand>
    {
        public CompleteTaskCommandValidator()
        {
            RuleFor(x => x.TaskId)
                .NotEmpty().WithMessage("Task Id is required.");

            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("Project Id is required.");
        }

    }
}
