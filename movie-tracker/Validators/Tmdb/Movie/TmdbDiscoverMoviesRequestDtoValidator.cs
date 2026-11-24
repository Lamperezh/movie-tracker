using FluentValidation;
using Persistence.DTOs.Tmdb.Movie;

namespace movie_tracker.Validators.Tmdb.Movie;

public class TmdbDiscoverMoviesRequestDtoValidator : AbstractValidator<TmdbDiscoverMoviesRequestDto>
{
    public TmdbDiscoverMoviesRequestDtoValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be greater than or equal to 1.");

        RuleFor(x => x.PrimaryReleaseYear)
            .InclusiveBetween(1874, 2100)
            .When(x => x.PrimaryReleaseYear.HasValue)
            .WithMessage("Invalid release year.");

        RuleFor(x => x.VoteAverageGte)
            .InclusiveBetween(0, 10)
            .When(x => x.VoteAverageGte.HasValue)
            .WithMessage("Vote average must be between 0 and 10.");

        RuleFor(x => x.VoteAverageLte)
            .InclusiveBetween(0, 10)
            .When(x => x.VoteAverageLte.HasValue)
            .WithMessage("Vote average must be between 0 and 10.");

        RuleFor(x => x)
            .Must(x =>
                !x.VoteAverageGte.HasValue ||
                !x.VoteAverageLte.HasValue ||
                x.VoteAverageGte <= x.VoteAverageLte)
            .WithMessage("Minimum vote average cannot exceed maximum.");

        RuleFor(x => x.VoteCountGte)
            .GreaterThanOrEqualTo(0)
            .When(x => x.VoteCountGte.HasValue)
            .WithMessage("Vote count cannot be negative.");

        RuleFor(x => x.VoteCountLte)
            .GreaterThanOrEqualTo(0)
            .When(x => x.VoteCountLte.HasValue)
            .WithMessage("Vote count cannot be negative.");

        RuleFor(x => x.WithRuntimeGte)
            .GreaterThanOrEqualTo(0)
            .When(x => x.WithRuntimeGte.HasValue)
            .WithMessage("Runtime cannot be negative.");

        RuleFor(x => x.WithRuntimeLte)
            .GreaterThanOrEqualTo(0)
            .When(x => x.WithRuntimeLte.HasValue)
            .WithMessage("Runtime cannot be negative.");

        RuleFor(x => x)
            .Must(x =>
                !x.ReleaseDateGte.HasValue ||
                !x.ReleaseDateLte.HasValue ||
                x.ReleaseDateGte <= x.ReleaseDateLte)
            .WithMessage("Start date cannot be later than end date.");

        var allowedSortOptions = new[]
        {
            "popularity.asc",
            "popularity.desc",
            "primary_release_date.asc",
            "primary_release_date.desc",
            "revenue.asc",
            "revenue.desc",
            "original_title.asc",
            "original_title.desc",
            "title.asc",
            "title.desc",
            "vote_average.asc",
            "vote_average.desc",
            "vote_count.asc",
            "vote_count.desc"
        };

        RuleFor(x => x.SortBy)
            .Must(sortBy =>
                string.IsNullOrWhiteSpace(sortBy) ||
                allowedSortOptions.Contains(sortBy))
            .WithMessage("Invalid sort option.");
    }
}