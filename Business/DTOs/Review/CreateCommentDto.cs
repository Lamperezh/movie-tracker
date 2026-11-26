using Business.Enums;

namespace Business.DTOs.Review;

public class CreateCommentDto
{
    public int TmdbId { get; set; }

    public MediaType Type { get; set; }

    public string Comment { get; set; } = string.Empty;
}