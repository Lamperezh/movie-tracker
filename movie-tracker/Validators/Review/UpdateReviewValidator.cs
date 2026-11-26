using Business.DTOs.Review;
using FluentValidation;

namespace movie_tracker.Validators.Review;

public class UpdateReviewValidator : AbstractValidator<UpdateReviewDto>
{
    public UpdateReviewValidator()
    {
        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 10)
            .WithMessage("Rating must be between 1 and 10.");
    }
}