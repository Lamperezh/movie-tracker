using System.Text.Json.Serialization;

namespace movie_tracker.DTOs.Tmdb.Actor;

public class TmdbTvCreditsDto
{
    [JsonPropertyName("cast")]
    public List<TmdbTvCreditDto> Cast { get; set; } = new();

    [JsonPropertyName("crew")]
    public List<TmdbTvCreditDto> Crew { get; set; } = new();
}