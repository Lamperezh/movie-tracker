namespace movie_tracker.Models;

public enum MediaType
{
    Movie = 1,
    Series = 2
}

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public MediaType Type { get; set; } // Фильм или Сериал
    public int? DurationMinutes { get; set; }
    public string Genre { get; set; } = string.Empty;
    public double Rating { get; set; }
    public string? PosterUrl { get; set; }

    // Навигационные свойства
    public ICollection<Actor> Actors { get; set; } = new List<Actor>();
    public ICollection<WatchlistItem> WatchlistItems { get; set; } = new List<WatchlistItem>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}