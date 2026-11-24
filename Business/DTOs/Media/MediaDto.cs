namespace Business.DTOs.Media;

public class MediaDto
{
    public int Id { get; set; }
    
    public int TmdbId { get; set; }
    
    public int Type { get; set; }
    
    public List<string> OstLinks { get; set; } = new();
}