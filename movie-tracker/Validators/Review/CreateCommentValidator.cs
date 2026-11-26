using Business.DTOs.Review;
using FluentValidation;

namespace movie_tracker.Validators.Review;

public class CreateCommentValidator : AbstractValidator<CreateCommentDto>
{
    public CreateCommentValidator()
    {
        RuleFor(x => x.TmdbId)
            .GreaterThan(0)
            .WithMessage("TMDB ID must be greater than 0.");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Invalid media type.");

        RuleFor(x => x.Comment)
            .NotEmpty()
            .WithMessage("Comment cannot be empty.")
            .MaximumLength(2000)
            .WithMessage("Comment cannot exceed 2000 characters.");
    }
}