using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Comments.Commands.UpdateComment
{
    public class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
    {
        public UpdateCommentCommandValidator()
        {
            RuleFor(x => x.TaskId)
                .NotEmpty().WithMessage("TaskId is required.");

            RuleFor(x => x.CommentId)
                .NotEmpty().WithMessage("CommentId is required.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MaximumLength(500).WithMessage("Content must not exceed 500 characters.");
        }
    }
}
