using System.Text.Json.Serialization;

namespace movie_tracker.DTOs.Tmdb.Actor;

public class TmdbTvCreditDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

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

    [JsonPropertyName("first_air_date")]
    public string? FirstAirDate { get; set; }

    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }

    [JsonPropertyName("episode_count")]
    public int? EpisodeCount { get; set; }

    [JsonPropertyName("total_episode_count")]
    public int? TotalEpisodeCount { get; set; }
}