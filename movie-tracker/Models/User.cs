using Microsoft.AspNetCore.Identity;

namespace movie_tracker.Models;

public class User : IdentityUser<int>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<WatchlistItem> WatchlistItems { get; set; } = new List<WatchlistItem>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}