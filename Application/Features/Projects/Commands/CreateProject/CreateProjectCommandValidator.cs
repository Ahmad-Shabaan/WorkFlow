

using FluentValidation;

namespace Application.Features.Projects.Commands.CreateProject
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {

            RuleFor(p => p.ProjectName)
                .NotEmpty().WithMessage("Project name is required.")
                .MaximumLength(100).WithMessage("Project name must not exceed 100 characters.");

            RuleFor(p => p.ProjectDescription)
                 .NotEmpty().WithMessage("Project description is required.")
                .MaximumLength(500).WithMessage("Project description must not exceed 500 characters.");

            RuleFor(p => p.StartDate)
                .NotEmpty().WithMessage("Start date is required.")
                .GreaterThanOrEqualTo(DateTimeOffset.Now).WithMessage("Start date cannot be in the past.");

            RuleFor(p => p.EndDate)
                .NotEmpty().WithMessage("End date is required.")
                .GreaterThanOrEqualTo(p => p.StartDate).WithMessage("End date cannot be earlier than start date.");

            RuleFor(p => p.ManagerId)
                 .NotEmpty().WithMessage("Manager is required.");
        }
    }
}
