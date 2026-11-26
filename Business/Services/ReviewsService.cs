
using Business.DTOs.Review;
using Business.Enums;
using Business.Models;
using Business.Repositories;
using Business.Services.Interfaces;

namespace Business.Services;

public class ReviewsService : IReviewsService
{
    private readonly IReviewRepository _repository;

    public ReviewsService(IReviewRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ReviewDto>> GetAllAsync(
        int? userId,
        int? tmdbId,
        MediaType? type,
        int? currentUserId = null)
    {
        ValidateOptionalIds(userId, tmdbId, currentUserId);

        if (type.HasValue && !Enum.IsDefined(typeof(MediaType), type.Value))
            throw new ArgumentException("Invalid media type.");

        var reviews = await _repository.GetAllAsync(userId, tmdbId, type);

        return reviews.Select(x => ToDto(x, currentUserId));
    }

    public async Task<ReviewDto> GetByIdAsync(int id, int? currentUserId = null)
    {
        if (id <= 0)
            throw new ArgumentException("Review ID must be greater than 0.");

        if (currentUserId.HasValue && currentUserId.Value <= 0)
            throw new ArgumentException("User ID must be greater than 0.");

        var review = await _repository.GetByIdAsync(id);

        if (review == null)
            throw new KeyNotFoundException("Review not found.");

        return ToDto(review, currentUserId);
    }

    public async Task<ReviewDto> CreateRatingAsync(CreateReviewDto dto, int userId)
    {
        ValidateUserId(userId);
        ValidateMedia(dto.TmdbId, dto.Type);

        await EnsureUserExistsAsync(userId);

        var review = await _repository.GetByUserAndMediaAsync(
            userId,
            dto.TmdbId,
            dto.Type);

        if (review == null)
        {
            review = new Review
            {
                UserId = userId,
                TmdbId = dto.TmdbId,
                Type = dto.Type,
                Rating = dto.Rating,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(review);
        }
        else
        {
            if (review.Rating.HasValue)
                throw new InvalidOperationException(
                    "You have already rated this media.");

            review.Rating = dto.Rating;
        }

        await _repository.SaveChangesAsync();

        var savedReview = await _repository.GetByIdAsync(review.Id);

        return ToDto(savedReview!, userId);
    }

    public async Task<ReviewDto> UpdateRatingAsync(int id, UpdateReviewDto dto, int userId)
    {
        ValidateReviewAndUserIds(id, userId);

        var review = await GetOwnedReviewAsync(id, userId);

        review.Rating = dto.Rating;

        await SaveOrDeleteEmptyReviewAsync(review);

        return ToDto(review, userId);
    }

    public async Task DeleteRatingAsync(int id, int userId)
    {
        ValidateReviewAndUserIds(id, userId);

        var review = await GetOwnedReviewAsync(id, userId);

        review.Rating = null;

        await SaveOrDeleteEmptyReviewAsync(review);
    }

    public async Task<ReviewDto> CreateCommentAsync(CreateCommentDto dto, int userId)
    {
        ValidateUserId(userId);
        ValidateMedia(dto.TmdbId, dto.Type);
        ValidateComment(dto.Comment);

        await EnsureUserExistsAsync(userId);

        var review = await _repository.GetByUserAndMediaAsync(
            userId,
            dto.TmdbId,
            dto.Type);

        if (review == null)
        {
            review = new Review
            {
                UserId = userId,
                TmdbId = dto.TmdbId,
                Type = dto.Type,
                Comment = dto.Comment.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(review);
        }
        else
        {
            if (!string.IsNullOrEmpty(review.Comment))
                throw new InvalidOperationException(
                    "You have already commented on this media.");

            review.Comment = dto.Comment.Trim();
        }

        await _repository.SaveChangesAsync();

        var savedReview = await _repository.GetByIdAsync(review.Id);

        return ToDto(savedReview!, userId);
    }

    public async Task<ReviewDto> UpdateCommentAsync(int id, UpdateCommentDto dto, int userId)
    {
        ValidateReviewAndUserIds(id, userId);
        ValidateComment(dto.Comment);

        var review = await GetOwnedReviewAsync(id, userId);

        if (string.IsNullOrEmpty(review.Comment))
            throw new InvalidOperationException(
                "Comment does not exist.");

        review.Comment = dto.Comment.Trim();

        await _repository.SaveChangesAsync();

        return ToDto(review, userId);
    }

    public async Task DeleteCommentAsync(int id, int userId)
    {
        ValidateReviewAndUserIds(id, userId);

        var review = await GetOwnedReviewAsync(id, userId);

        if (string.IsNullOrEmpty(review.Comment))
            throw new InvalidOperationException(
                "Comment does not exist.");

        _repository.DeleteReactions(review.Reactions);
        review.Reactions.Clear();
        review.Comment = null;

        await SaveOrDeleteEmptyReviewAsync(review);
    }

    public async Task<ReviewDto> SetReactionAsync(int reviewId, SetCommentReactionDto dto, int userId)
    {
        ValidateReviewAndUserIds(reviewId, userId);

        await EnsureUserExistsAsync(userId);

        var review = await _repository.GetByIdAsync(reviewId);

        if (review == null)
            throw new KeyNotFoundException("Review not found.");

        if (string.IsNullOrEmpty(review.Comment))
            throw new InvalidOperationException(
                "Cannot react to a review without a comment.");

        if (review.UserId == userId)
            throw new InvalidOperationException(
                "You cannot react to your own comment.");

        var reaction = await _repository.GetReactionAsync(reviewId, userId);

        if (reaction == null)
        {
            reaction = new CommentReaction
            {
                ReviewId = reviewId,
                UserId = userId,
                IsLike = dto.IsLike
            };

            await _repository.AddReactionAsync(reaction);
            review.Reactions.Add(reaction);
        }
        else
        {
            reaction.IsLike = dto.IsLike;
        }

        await _repository.SaveChangesAsync();

        return ToDto(review, userId);
    }

    public async Task DeleteReactionAsync(int reviewId, int userId)
    {
        ValidateReviewAndUserIds(reviewId, userId);

        var review = await _repository.GetByIdAsync(reviewId);

        if (review == null)
            throw new KeyNotFoundException("Review not found.");

        var reaction = await _repository.GetReactionAsync(reviewId, userId);

        if (reaction == null)
            throw new KeyNotFoundException("Reaction not found.");

        _repository.DeleteReaction(reaction);
        review.Reactions.Remove(reaction);

        await _repository.SaveChangesAsync();
    }

    private async Task<Review> GetOwnedReviewAsync(int id, int userId)
    {
        var review = await _repository.GetByIdAndUserIdAsync(id, userId);

        if (review == null)
            throw new KeyNotFoundException("Review not found.");

        return review;
    }

    private async Task EnsureUserExistsAsync(int userId)
    {
        var exists = await _repository.UserExistsAsync(userId);

        if (!exists)
            throw new KeyNotFoundException("User not found.");
    }

    private async Task SaveOrDeleteEmptyReviewAsync(Review review)
    {
        if (!review.Rating.HasValue && string.IsNullOrEmpty(review.Comment))
            _repository.Delete(review);

        await _repository.SaveChangesAsync();
    }

    private static void ValidateOptionalIds(int? userId, int? tmdbId, int? currentUserId)
    {
        if (userId.HasValue && userId.Value <= 0)
            throw new ArgumentException("User ID must be greater than 0.");

        if (tmdbId.HasValue && tmdbId.Value <= 0)
            throw new ArgumentException("TMDB ID must be greater than 0.");

        if (currentUserId.HasValue && currentUserId.Value <= 0)
            throw new ArgumentException("User ID must be greater than 0.");
    }

    private static void ValidateUserId(int userId)
    {
        if (userId <= 0)
            throw new ArgumentException("User ID must be greater than 0.");
    }

    private static void ValidateReviewAndUserIds(int reviewId, int userId)
    {
        if (reviewId <= 0 || userId <= 0)
            throw new ArgumentException(
                "Review ID and User ID must be greater than 0.");
    }

    private static void ValidateMedia(int tmdbId, MediaType type)
    {
        if (tmdbId <= 0)
            throw new ArgumentException("TMDB ID must be greater than 0.");

        if (!Enum.IsDefined(typeof(MediaType), type))
            throw new ArgumentException("Invalid media type.");
    }

    private static void ValidateComment(string comment)
    {
        if (string.IsNullOrWhiteSpace(comment))
            throw new ArgumentException("Comment cannot be empty.");
    }

    private static ReviewDto ToDto(Review review, int? currentUserId = null)
    {
        CommentReactionDto? reactions = null;

        if (!string.IsNullOrEmpty(review.Comment))
        {
            reactions = new CommentReactionDto
            {
                LikeCount = review.Reactions.Count(x => x.IsLike),
                DislikeCount = review.Reactions.Count(x => !x.IsLike),
                CurrentUserReaction = currentUserId.HasValue
                    ? review.Reactions
                        .Where(x => x.UserId == currentUserId.Value)
                        .Select(x => (bool?)x.IsLike)
                        .FirstOrDefault()
                    : null
            };
        }

        return new ReviewDto
        {
            Id = review.Id,
            UserId = review.UserId,
            UserName = review.User?.UserName ?? string.Empty,
            TmdbId = review.TmdbId,
            Type = review.Type,
            Rating = review.Rating,
            Comment = review.Comment,
            CreatedAt = review.CreatedAt,
            Reactions = reactions
        };
    }
}