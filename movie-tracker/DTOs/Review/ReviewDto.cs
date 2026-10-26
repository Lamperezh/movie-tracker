using movie_tracker.Models;

namespace movie_tracker.DTOs.Review;

public class ReviewDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int TmdbId { get; set; }

    public MediaType Type { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; }
}