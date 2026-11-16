using FluentValidation;
using movie_tracker.DTOs.Media;

namespace movie_tracker.Validators.Media;

public class CreateMediaDtoValidator : AbstractValidator<CreateMediaDto>
{
    public CreateMediaDtoValidator()
    {
        RuleFor(x => x.TmdbId)
            .GreaterThan(0)
            .WithMessage("TMDB ID must be greater than 0.");

        RuleFor(x => x.Type)
            .Must(type => type == 1 || type == 2)
            .WithMessage("Type must be 1 (Movie) or 2 (Series).");
    }
}