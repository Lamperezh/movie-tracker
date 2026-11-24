using Business.DTOs.Review;
using Business.Enums;
using Business.Models;
using Business.Repositories;

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
        MediaType? type)
    {
        if (userId.HasValue && userId.Value <= 0)
            throw new ArgumentException("User ID must be greater than 0.");

        if (tmdbId.HasValue && tmdbId.Value <= 0)
            throw new ArgumentException("TMDB ID must be greater than 0.");

        if (type.HasValue && !Enum.IsDefined(typeof(MediaType), type.Value))
            throw new ArgumentException("Invalid media type.");

        var reviews = await _repository.GetAllAsync(userId, tmdbId, type);

        return reviews.Select(ToDto);
    }

    public async Task<ReviewDto> GetByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Review ID must be greater than 0.");

        var review = await _repository.GetByIdAsync(id);

        if (review == null)
            throw new KeyNotFoundException("Review not found.");

        return ToDto(review);
    }

    public async Task<ReviewDto> CreateAsync(CreateReviewDto dto, int userId)
    {
        if (userId <= 0)
            throw new ArgumentException("User ID must be greater than 0.");

        var userExists = await _repository.UserExistsAsync(userId);

        if (!userExists)
            throw new KeyNotFoundException("User not found.");

        var alreadyExists = await _repository.ExistsAsync(
            userId, dto.TmdbId, dto.Type);

        if (alreadyExists)
            throw new InvalidOperationException("You have already reviewed this media.");

        var review = new Review
        {
            UserId = userId,
            TmdbId = dto.TmdbId,
            Type = dto.Type,
            Rating = dto.Rating,
            Comment = dto.Comment?.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(review);
        await _repository.SaveChangesAsync();

        return ToDto(review);
    }

    public async Task<ReviewDto> UpdateAsync(
        int id,
        UpdateReviewDto dto,
        int userId)
    {
        if (id <= 0 || userId <= 0)
            throw new ArgumentException("Review ID and User ID must be greater than 0.");

        var review = await _repository.GetByIdAndUserIdAsync(id, userId);

        if (review == null)
            throw new KeyNotFoundException("Review not found.");

        var duplicateExists = await _repository.ExistsAsync(
            userId, dto.TmdbId, dto.Type, id);

        if (duplicateExists)
            throw new InvalidOperationException("You have already reviewed this media.");

        review.TmdbId = dto.TmdbId;
        review.Type = dto.Type;
        review.Rating = dto.Rating;
        review.Comment = dto.Comment?.Trim();

        await _repository.SaveChangesAsync();

        return ToDto(review);
    }

    public async Task DeleteAsync(int id, int userId)
    {
        if (id <= 0 || userId <= 0)
            throw new ArgumentException("Review ID and User ID must be greater than 0.");

        var review = await _repository.GetByIdAndUserIdAsync(id, userId);

        if (review == null)
            throw new KeyNotFoundException("Review not found.");

        _repository.Delete(review);
        await _repository.SaveChangesAsync();
    }

    private static ReviewDto ToDto(Review review)
    {
        return new ReviewDto
        {
            Id = review.Id,
            UserId = review.UserId,
            TmdbId = review.TmdbId,
            Type = review.Type,
            Rating = review.Rating,
            Comment = review.Comment,
            CreatedAt = review.CreatedAt
        };
    }
}