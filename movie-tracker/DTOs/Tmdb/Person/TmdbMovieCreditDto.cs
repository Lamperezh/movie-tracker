using System.Text.Json.Serialization;

namespace movie_tracker.DTOs.Tmdb.Actor;

public class TmdbMovieCreditDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("character")]
    public string? Character { get; set; }

    [JsonPropertyName("credit_id")]
    public string? CreditId { get; set; }

    [JsonPropertyName("order")]
    public int? Order { get; set; }

    [JsonPropertyName("job")]
    public string? Job { get; set; }

    [JsonPropertyName("department")]
    public string? Department { get; set; }

    [JsonPropertyName("release_date")]
    public string? ReleaseDate { get; set; }

    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }
}