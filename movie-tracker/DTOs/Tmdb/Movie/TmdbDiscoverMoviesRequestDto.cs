namespace movie_tracker.DTOs.Tmdb.Movie;

public class TmdbDiscoverMoviesRequestDto
{
    public int Page { get; set; } = 1;

    public string? SortBy { get; set; }

    public string? WithGenres { get; set; }
    public string? WithoutGenres { get; set; }

    public int? PrimaryReleaseYear { get; set; }

    public DateTime? ReleaseDateGte { get; set; }
    public DateTime? ReleaseDateLte { get; set; }

    public double? VoteAverageGte { get; set; }
    public double? VoteAverageLte { get; set; }

    public int? VoteCountGte { get; set; }
    public int? VoteCountLte { get; set; }

    public int? WithRuntimeGte { get; set; }
    public int? WithRuntimeLte { get; set; }

    public string? WithOriginalLanguage { get; set; }

    public string Language { get; set; } = "uk-UA";
    public string? Region { get; set; }

    public bool IncludeAdult { get; set; } = false;
    public bool IncludeVideo { get; set; } = false;
}