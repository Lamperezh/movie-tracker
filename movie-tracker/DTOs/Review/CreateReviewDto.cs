using movie_tracker.Models;

namespace movie_tracker.DTOs.Review;

public class CreateReviewDto
{
    public int TmdbId { get; set; }

    public MediaType Type { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }
}