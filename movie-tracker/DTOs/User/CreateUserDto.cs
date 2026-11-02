using System.ComponentModel.DataAnnotations;

namespace movie_tracker.DTOs.User;

public class CreateUserDto
{
    [Required]
    public string UserName { get; set; } = string.Empty; // Большая буква N

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}