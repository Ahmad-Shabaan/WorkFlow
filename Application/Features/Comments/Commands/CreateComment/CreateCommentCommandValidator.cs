
using FluentValidation;

namespace Application.Features.Comments.Commands.CreateComment
{
    public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentCommandValidator()
        {
            RuleFor(x => x.TaskId)
                .NotEmpty().WithMessage("TaskId is required.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MaximumLength(500).WithMessage("Content must not exceed 500 characters.");

        }
    }
}
