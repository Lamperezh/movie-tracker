using System.Text.Json.Serialization;

namespace movie_tracker.DTOs.Tmdb.Actor;

public class TmdbPersonDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("biography")]
    public string Biography { get; set; } = string.Empty;

    [JsonPropertyName("birthday")]
    public string? Birthday { get; set; }
    
    [JsonPropertyName("deathday")]
    public string? Deathday { get; set; }

    [JsonPropertyName("place_of_birth")]
    public string? PlaceOfBirth { get; set; }

    [JsonPropertyName("profile_path")]
    public string? ProfilePath { get; set; }

    [JsonPropertyName("known_for_department")]
    public string? KnownForDepartment { get; set; }

    [JsonPropertyName("also_known_as")]
    public List<string> AlsoKnownAs { get; set; } = new();
    
    [JsonPropertyName("movie_credits")]
    public TmdbMovieCreditsDto? MovieCredits { get; set; }
    
    [JsonPropertyName("tv_credits")]
    public TmdbTvCreditsDto? TvCredits { get; set; }
}