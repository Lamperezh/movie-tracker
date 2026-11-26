namespace Business.DTOs.Review;

public class CommentReactionDto
{
    public int LikeCount { get; set; }

    public int DislikeCount { get; set; }

    public bool? CurrentUserReaction { get; set; }
}