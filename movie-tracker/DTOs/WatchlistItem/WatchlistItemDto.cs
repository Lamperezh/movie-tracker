using movie_tracker.Models;

namespace movie_tracker.DTOs.WatchlistItem;

public class WatchlistItemDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int TmdbId { get; set; }

    public MediaType Type { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? PosterPath { get; set; }

    public WatchStatus Status { get; set; }

    public DateTime AddedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}