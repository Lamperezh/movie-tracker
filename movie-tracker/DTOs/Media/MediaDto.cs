using movie_tracker.Models;

namespace movie_tracker.DTOs.Media;

public class MediaDto
{
    public int Id { get; set; }

    public int TmdbId { get; set; }

    public MediaType Type { get; set; }

    public List<string> OstLinks { get; set; } = new();

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}