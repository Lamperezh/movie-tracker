using System.Text.Json.Serialization;

namespace movie_tracker.DTOs.Tmdb.Movie;

public class TmdbCreditMemberDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("original_name")]
    public string OriginalName { get; set; } = string.Empty;

    [JsonPropertyName("character")]
    public string? Character { get; set; }

    [JsonPropertyName("department")]
    public string? Department { get; set; }

    [JsonPropertyName("job")]
    public string? Job { get; set; }

    [JsonPropertyName("profile_path")]
    public string? ProfilePath { get; set; }

    [JsonPropertyName("credit_id")]
    public string? CreditId { get; set; }

    [JsonPropertyName("order")]
    public int? Order { get; set; }

    [JsonPropertyName("gender")]
    public int Gender { get; set; }

    [JsonPropertyName("known_for_department")]
    public string? KnownForDepartment { get; set; }
}

public class TmdbCreditsResponseDto
{
    [JsonPropertyName("cast")]
    public List<TmdbCreditMemberDto> Cast { get; set; } = new();

    [JsonPropertyName("crew")]
    public List<TmdbCreditMemberDto> Crew { get; set; } = new();
}