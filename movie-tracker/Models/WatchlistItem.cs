namespace movie_tracker.Models;

public class WatchlistItem
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int TmdbId { get; set; }

    public MediaType Type { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? PosterPath { get; set; }

    public WatchStatus Status { get; set; } = WatchStatus.Planned;

    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}

public enum WatchStatus
{
    Planned,
    Watching,
    Completed,
    Dropped,
    OnHold
}