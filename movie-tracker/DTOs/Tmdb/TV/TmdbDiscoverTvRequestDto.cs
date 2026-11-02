namespace movie_tracker.DTOs.Tmdb.TV;

public class TmdbDiscoverTvRequestDto
{
    public int Page { get; set; } = 1;

    public string? SortBy { get; set; }

    public string? WithGenres { get; set; }
    public string? WithoutGenres { get; set; }

    public int? FirstAirDateYear { get; set; }

    public DateTime? FirstAirDateGte { get; set; }
    public DateTime? FirstAirDateLte { get; set; }

    public DateTime? AirDateGte { get; set; }
    public DateTime? AirDateLte { get; set; }

    public double? VoteAverageGte { get; set; }
    public double? VoteAverageLte { get; set; }

    public int? VoteCountGte { get; set; }
    public int? VoteCountLte { get; set; }

    public int? WithRuntimeGte { get; set; }
    public int? WithRuntimeLte { get; set; }

    public string? WithOriginalLanguage { get; set; }

    public string? WithNetworks { get; set; }
    public string? WithOriginCountry { get; set; }
    public string? WithStatus { get; set; }
    public string? WithType { get; set; }

    public string Language { get; set; } = "uk-UA";
    public string? Timezone { get; set; }

    public bool IncludeAdult { get; set; } = false;
    public bool IncludeNullFirstAirDates { get; set; } = false;
    public bool? ScreenedTheatrically { get; set; }
}