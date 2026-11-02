using movie_tracker.Models;

namespace movie_tracker.DTOs.Media;

public class UpdateMediaDto
{
    public int TmdbId { get; set; }

    public MediaType Type { get; set; }

    public List<string> OstLinks { get; set; } = new();
}