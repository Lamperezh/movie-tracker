using FluentValidation;
using movie_tracker.DTOs.Review;

namespace movie_tracker.Validators.Review;

public class UpdateReviewValidator : AbstractValidator<UpdateReviewDto>
{
    public UpdateReviewValidator()
    {
        RuleFor(x => x.TmdbId)
            .GreaterThan(0)
            .WithMessage("TMDB ID must be greater than 0.");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Invalid media type.");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 10)
            .WithMessage("Rating must be between 1 and 10.");
    }
}