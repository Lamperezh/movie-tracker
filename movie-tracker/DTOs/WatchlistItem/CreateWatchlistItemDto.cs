using movie_tracker.Models;

namespace movie_tracker.DTOs.WatchlistItem;

public class CreateWatchlistItemDto
{
    public int TmdbId { get; set; }

    public MediaType Type { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? PosterPath { get; set; }

    public WatchStatus Status { get; set; } = WatchStatus.Planned;
}