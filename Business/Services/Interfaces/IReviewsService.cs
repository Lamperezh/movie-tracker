using Business.DTOs.Review;
using Business.Enums;

namespace Business.Services.Interfaces;

public interface IReviewsService
{
    Task<IEnumerable<ReviewDto>> GetAllAsync(
        int? userId,
        int? tmdbId,
        MediaType? type,
        int? currentUserId = null);

    Task<ReviewDto> GetByIdAsync(
        int id,
        int? currentUserId = null);

    Task<ReviewDto> CreateRatingAsync(CreateReviewDto dto, int userId);
    Task<ReviewDto> UpdateRatingAsync(int id, UpdateReviewDto dto, int userId);
    Task DeleteRatingAsync(int id, int userId);

    Task<ReviewDto> CreateCommentAsync(CreateCommentDto dto, int userId);
    Task<ReviewDto> UpdateCommentAsync(int id, UpdateCommentDto dto, int userId);
    Task DeleteCommentAsync(int id, int userId);

    Task<ReviewDto> SetReactionAsync(
        int reviewId,
        SetCommentReactionDto dto,
        int userId);

    Task DeleteReactionAsync(int reviewId, int userId);
}