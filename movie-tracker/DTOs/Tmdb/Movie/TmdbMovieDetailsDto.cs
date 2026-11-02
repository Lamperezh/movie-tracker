using System.Text.Json.Serialization;

namespace movie_tracker.DTOs.Tmdb.Movie;

public class TmdbMovieDetailsDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("original_title")]
    public string OriginalTitle { get; set; } = string.Empty;

    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    [JsonPropertyName("release_date")]
    public string? ReleaseDate { get; set; }

    [JsonPropertyName("runtime")]
    public int? Runtime { get; set; }

    [JsonPropertyName("genres")]
    public List<TmdbGenreDto> Genres { get; set; } = new();

    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }

    [JsonPropertyName("backdrop_path")]
    public string? BackdropPath { get; set; }

    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }

    [JsonPropertyName("vote_count")]
    public int VoteCount { get; set; }

    [JsonPropertyName("original_language")]
    public string OriginalLanguage { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("budget")]
    public long Budget { get; set; }

    [JsonPropertyName("revenue")]
    public long Revenue { get; set; }
    
    [JsonPropertyName("credits")]
    public TmdbCreditsResponseDto? Credits { get; set; }
}

public class TmdbGenreDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}