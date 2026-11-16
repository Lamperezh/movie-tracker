using movie_tracker.Models;

namespace movie_tracker.DTOs.Media;

public class UpdateMediaDto
{
    public List<string> OstLinks { get; set; } = new();
}