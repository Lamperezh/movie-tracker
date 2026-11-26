using Microsoft.AspNetCore.Identity;

namespace Business.Models;

public class User : IdentityUser<int>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<WatchlistItem> WatchlistItems { get; set; } = new List<WatchlistItem>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<CommentReaction> CommentReactions { get; set; } = new List<CommentReaction>();
}