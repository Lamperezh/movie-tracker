using Business.DTOs.WatchlistItem;
using FluentValidation;

namespace movie_tracker.Validators.WatchlistItem;

public class UpdateWatchlistItemValidator : AbstractValidator<UpdateWatchlistItemDto>
{
    public UpdateWatchlistItemValidator()
    {
        RuleFor(x => x.TmdbId)
            .GreaterThan(0)
            .WithMessage("TMDB ID must be greater than 0.");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Invalid media type.");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Invalid watch status.");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.");
    }
}