using FluentValidation;
using Persistence.DTOs.Tmdb.TV;

namespace movie_tracker.Validators.Tmdb.TV;

public class TmdbDiscoverTvRequestDtoValidator : AbstractValidator<TmdbDiscoverTvRequestDto>
{
    public TmdbDiscoverTvRequestDtoValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be greater than or equal to 1.");

        RuleFor(x => x.FirstAirDateYear)
            .InclusiveBetween(1874, 2100)
            .When(x => x.FirstAirDateYear.HasValue)
            .WithMessage("Invalid first air date year.");

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
                !x.FirstAirDateGte.HasValue ||
                !x.FirstAirDateLte.HasValue ||
                x.FirstAirDateGte <= x.FirstAirDateLte)
            .WithMessage("First air date start cannot be later than end date.");

        RuleFor(x => x)
            .Must(x =>
                !x.AirDateGte.HasValue ||
                !x.AirDateLte.HasValue ||
                x.AirDateGte <= x.AirDateLte)
            .WithMessage("Air date start cannot be later than end date.");

        var allowedSortOptions = new[]
        {
            "popularity.asc",
            "popularity.desc",
            "first_air_date.asc",
            "first_air_date.desc",
            "vote_average.asc",
            "vote_average.desc",
            "vote_count.asc",
            "vote_count.desc",
            "original_name.asc",
            "original_name.desc",
            "name.asc",
            "name.desc"
        };

        RuleFor(x => x.SortBy)
            .Must(sortBy =>
                string.IsNullOrWhiteSpace(sortBy) ||
                allowedSortOptions.Contains(sortBy))
            .WithMessage("Invalid sort option.");
    }
}