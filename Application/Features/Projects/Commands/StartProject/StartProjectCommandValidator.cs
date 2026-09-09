using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Projects.Commands.StartProject
{
    public class StartProjectCommandValidator : AbstractValidator<StartProjectCommand>
    {
        public StartProjectCommandValidator()
        {

            RuleFor(p => p.ProjectId)
                .NotEmpty().WithMessage("Project Id is required.");
        }
    }
}
