namespace movie_tracker.Models;

public class WatchlistItem
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int MovieId { get; set; }
    public WatchStatus Status { get; set; }
}

public enum WatchStatus
{
    Watching,
    Completed,
    Planned,
    Dropped,
    OnHold
}