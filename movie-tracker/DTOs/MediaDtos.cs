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

    [Required]
    public int Type { get; set; } // 1 - Movie, 2 - Series
}

public class UpdateMediaDto
{
    public List<string> OstLinks { get; set; } = new();
}