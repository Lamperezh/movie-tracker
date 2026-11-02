using System.ComponentModel.DataAnnotations;

namespace movie_tracker.DTOs.User;

public class UpdateUserDto
{
    public string UserName { get; set; } = string.Empty; // Большая буква N

    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}