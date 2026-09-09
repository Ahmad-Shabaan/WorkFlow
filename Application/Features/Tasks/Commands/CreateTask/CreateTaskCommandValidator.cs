

using FluentValidation;

namespace Application.Features.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidator()
        {
            RuleFor(t => t.ProjectId)
                .NotEmpty().WithMessage("Project Id is required.");

            RuleFor(t => t.TaskName)
                .NotEmpty().WithMessage("TaskName is required.")
                .MaximumLength(100).WithMessage("TaskName must not exceed 100 characters.");

            RuleFor(t => t.TaskDescription)
                .NotEmpty().WithMessage("TaskDescription is required.")
                .MaximumLength(500).WithMessage("TaskDescription must not exceed 500 characters.");

            RuleFor(t => t.StartDate)
                .NotEmpty().WithMessage("StartDate is required.")
                .GreaterThanOrEqualTo(DateTimeOffset.Now).WithMessage("Start date cannot be in the past.");

            RuleFor(t => t.EndDate)
                .NotEmpty().WithMessage("EndDate is required.")
                .GreaterThan(x => x.StartDate).WithMessage("EndDate must be greater than StartDate.");
        }
    }
}
