using movie_tracker.Models;

namespace movie_tracker.DTOs.WatchlistItem;

public class WatchlistItemDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int MovieId { get; set; }
    public WatchStatus Status { get; set; }
}