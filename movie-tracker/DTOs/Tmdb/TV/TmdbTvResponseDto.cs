using System.Text.Json.Serialization;

namespace movie_tracker.DTOs.Tmdb.TV;

public class TmdbTvResponseDto
{
    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("results")]
    public List<TmdbTvDto> Results { get; set; } = new();

    [JsonPropertyName("total_pages")]
    public int TotalPages { get; set; }

    [JsonPropertyName("total_results")]
    public int TotalResults { get; set; }
}