
using FluentValidation;

namespace Application.Features.Projects.Commands.CompleteProject
{
    public class CompleteProjectCommandValidator : AbstractValidator<CompleteProjectCommand>
    {
        public CompleteProjectCommandValidator()
        {

            RuleFor(p => p.ProjectId)
                .NotEmpty().WithMessage("Project Id is required.");
        }
    }
}
