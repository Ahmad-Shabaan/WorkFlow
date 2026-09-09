using FluentValidation;

namespace Application.Features.Projects.Commands.CancelProject
{
    public class CancelProjectCommandValidator : AbstractValidator<CancelProjectCommand>
    {
        public CancelProjectCommandValidator()
        {

            RuleFor(p => p.ProjectId)
                .NotEmpty().WithMessage("Project Id is required.");
        }
    }
}
