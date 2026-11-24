using System.Text.Json.Serialization;

namespace Persistence.DTOs.Tmdb.Actor;

public class TmdbMovieCreditsDto
{
    [JsonPropertyName("cast")]
    public List<TmdbMovieCreditDto> Cast { get; set; } = new();

    [JsonPropertyName("crew")]
    public List<TmdbMovieCreditDto> Crew { get; set; } = new();
}