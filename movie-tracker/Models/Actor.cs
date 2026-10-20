namespace movie_tracker.Models;

public class Actor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Biography { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string? Country { get; set; }

    // Ќавигационное свойство дл€ св€зи с фильмами
    public ICollection<Movie> Movies { get; set; } = new List<Movie>();
}