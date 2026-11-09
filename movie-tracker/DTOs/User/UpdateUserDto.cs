using System.ComponentModel.DataAnnotations;

namespace movie_tracker.DTOs.User;

public class UpdateUserDto
{
    [Required]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}