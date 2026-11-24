using System.ComponentModel.DataAnnotations;

namespace Business.DTOs.Media;

public class CreateMediaDto
{
    [Required]
    public int TmdbId { get; set; }

    [Range(1, 2, ErrorMessage = "Type must be 1 (Movie) or 2 (Series).")]
    public int Type { get; set; }
}