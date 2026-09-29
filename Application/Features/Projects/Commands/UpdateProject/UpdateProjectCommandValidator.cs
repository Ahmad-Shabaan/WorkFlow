using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {
        public UpdateProjectCommandValidator()
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
