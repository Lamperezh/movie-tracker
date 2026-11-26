using Business.Enums;

namespace Business.DTOs.Review;

public class CreateReviewDto
{
    public int TmdbId { get; set; }

    public MediaType Type { get; set; }

    public int Rating { get; set; }
}