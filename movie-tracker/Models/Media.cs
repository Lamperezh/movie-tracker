namespace movie_tracker.Models;

public enum MediaType
{
    Movie = 1,
    Series = 2
}

public class Media
{
    public int Id { get; set; }

    public int TmdbId { get; set; }

    public MediaType Type { get; set; }

    public List<string> OstLinks { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}