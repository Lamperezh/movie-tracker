namespace movie_tracker.DTOs.User;

public class UserDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty; // Большая буква N
    public string Email { get; set; } = string.Empty;
}