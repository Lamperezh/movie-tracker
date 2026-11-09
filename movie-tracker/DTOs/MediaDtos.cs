using System.ComponentModel.DataAnnotations;

namespace movie_tracker.DTOs;

public class MediaDto
{
    public int Id { get; set; }
    public int TmdbId { get; set; }
    public int Type { get; set; }
    public List<string> OstLinks { get; set; } = new();
}

public class CreateMediaDto
{
    [Required]
    public int TmdbId { get; set; }

    [Range(1, 2, ErrorMessage = "Type must be 1 (Movie) or 2 (Series).")]
    public int Type { get; set; }
}

public class UpdateMediaDto
{
    public List<string> OstLinks { get; set; } = new();
}