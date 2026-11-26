using Business.DTOs.Review;
using FluentValidation;

namespace movie_tracker.Validators.Review;

public class UpdateCommentValidator : AbstractValidator<UpdateCommentDto>
{
    public UpdateCommentValidator()
    {
        RuleFor(x => x.Comment)
            .NotEmpty()
            .WithMessage("Comment cannot be empty.")
            .MaximumLength(2000)
            .WithMessage("Comment cannot exceed 2000 characters.");
    }
}