using System.Text.Json.Serialization;

namespace Persistence.DTOs.Tmdb.Movie;

public class TmdbVideosResponseDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("results")]
    public List<TmdbVideoDto> Results { get; set; } = new();
}

public class TmdbVideoDto
{
    [JsonPropertyName("iso_639_1")]
    public string Iso6391 { get; set; } = string.Empty;

    [JsonPropertyName("iso_3166_1")]
    public string Iso31661 { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("key")]
    public string Key { get; set; } = string.Empty;

    [JsonPropertyName("site")]
    public string Site { get; set; } = string.Empty;

    [JsonPropertyName("size")]
    public int Size { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("official")]
    public bool Official { get; set; }

    [JsonPropertyName("published_at")]
    public string PublishedAt { get; set; } = string.Empty;

    [JsonPropertyName("id")]
    public string VideoId { get; set; } = string.Empty;
}