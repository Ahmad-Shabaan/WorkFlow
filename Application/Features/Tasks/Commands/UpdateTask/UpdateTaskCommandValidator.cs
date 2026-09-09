
using FluentValidation;

namespace Application.Features.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
    {
        public UpdateTaskCommandValidator()
        {
            RuleFor(t=>t.ProjectId)
                .NotEmpty().WithMessage("Project Id is required.");
            RuleFor(t =>t.TaskId)
                .NotEmpty().WithMessage("Task Id is required.");

            RuleFor(t =>t.TaskName)
                .NotEmpty().WithMessage("Task Name is required.")
                .MaximumLength(100).WithMessage("Task Name must not exceed 100 characters.");

            RuleFor(t =>t.TaskDescription)
                .NotEmpty().WithMessage("Task Description is required.")
                .MaximumLength(500).WithMessage("Task Description must not exceed 500 characters.");

            RuleFor(t => t.StartDate)
                .NotEmpty().WithMessage("Start Date is required.")
                .GreaterThanOrEqualTo(DateTimeOffset.Now).WithMessage("Start Date cannot be in the past.");
            RuleFor(t => t.EndDate)
                .NotEmpty().WithMessage("End Date is required.")
                .GreaterThan(x => x.StartDate).WithMessage("End Date must be greater than Start Date.");

        }
    }
}
