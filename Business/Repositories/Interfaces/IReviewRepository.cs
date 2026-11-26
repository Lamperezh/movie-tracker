using Business.Enums;
using Business.Models;

namespace Business.Repositories;

public interface IReviewRepository
{
    Task<List<Review>> GetAllAsync(
        int? userId,
        int? tmdbId,
        MediaType? type);

    Task<Review?> GetByIdAsync(int id);

    Task<Review?> GetByIdAndUserIdAsync(int id, int userId);

    Task<Review?> GetByUserAndMediaAsync(
        int userId,
        int tmdbId,
        MediaType type);

    Task<CommentReaction?> GetReactionAsync(
        int reviewId,
        int userId);

    Task<bool> UserExistsAsync(int userId);

    Task<bool> ExistsAsync(
        int userId,
        int tmdbId,
        MediaType type,
        int? excludeId = null);

    Task AddAsync(Review review);

    Task AddReactionAsync(CommentReaction reaction);

    void Delete(Review review);

    void DeleteReaction(CommentReaction reaction);

    void DeleteReactions(IEnumerable<CommentReaction> reactions);

    Task SaveChangesAsync();
}