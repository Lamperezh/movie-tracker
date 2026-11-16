using Microsoft.EntityFrameworkCore;
using movie_tracker.Data;
using movie_tracker.DTOs.Review;
using movie_tracker.Enums;
using movie_tracker.Models;

namespace movie_tracker.Services;

public class ReviewsService : IReviewsService
{
    private readonly AppDbContext _context;

    public ReviewsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ReviewDto>> GetAllAsync(int? userId, int? tmdbId, MediaType? type)
    {
        var query = _context.Reviews.AsQueryable();

        if (userId.HasValue)
        {
            if (userId.Value <= 0) throw new ArgumentException("User ID must be greater than 0.");
            query = query.Where(x => x.UserId == userId.Value);
        }

        if (tmdbId.HasValue)
        {
            if (tmdbId.Value <= 0) throw new ArgumentException("TMDB ID must be greater than 0.");
            query = query.Where(x => x.TmdbId == tmdbId.Value);
        }

        if (type.HasValue)
        {
            if (!Enum.IsDefined(typeof(MediaType), type.Value)) throw new ArgumentException("Invalid media type.");
            query = query.Where(x => x.Type == type.Value);
        }

        var reviews = await query.OrderByDescending(x => x.CreatedAt).ToListAsync();
        return reviews.Select(ToDto);
    }

    public async Task<ReviewDto> GetByIdAsync(int id)
    {
        if (id <= 0) throw new ArgumentException("Review ID must be greater than 0.");

        var review = await _context.Reviews.FindAsync(id);
        if (review == null) throw new KeyNotFoundException("Review not found.");

        return ToDto(review);
    }

    public async Task<ReviewDto> CreateAsync(CreateReviewDto dto, int userId)
    {
        if (userId <= 0) throw new ArgumentException("User ID must be greater than 0.");

        var userExists = await _context.Set<User>().AnyAsync(x => x.Id == userId);
        if (!userExists) throw new KeyNotFoundException("User not found.");

        var alreadyExists = await _context.Reviews.AnyAsync(x => x.UserId == userId && x.TmdbId == dto.TmdbId && x.Type == dto.Type);
        if (alreadyExists) throw new InvalidOperationException("You have already reviewed this media.");

        var review = new Review
        {
            UserId = userId,
            TmdbId = dto.TmdbId,
            Type = dto.Type,
            Rating = dto.Rating,
            Comment = dto.Comment?.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        return ToDto(review);
    }

    public async Task<ReviewDto> UpdateAsync(int id, UpdateReviewDto dto, int userId)
    {
        if (id <= 0 || userId <= 0) throw new ArgumentException("Review ID and User ID must be greater than 0.");

        var review = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        if (review == null) throw new KeyNotFoundException("Review not found.");

        var duplicateExists = await _context.Reviews.AnyAsync(x => x.Id != id && x.UserId == userId && x.TmdbId == dto.TmdbId && x.Type == dto.Type);
        if (duplicateExists) throw new InvalidOperationException("You have already reviewed this media.");

        review.TmdbId = dto.TmdbId;
        review.Type = dto.Type;
        review.Rating = dto.Rating;
        review.Comment = dto.Comment?.Trim();

        await _context.SaveChangesAsync();
        return ToDto(review);
    }

    public async Task DeleteAsync(int id, int userId)
    {
        if (id <= 0 || userId <= 0) throw new ArgumentException("Review ID and User ID must be greater than 0.");

        var review = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        if (review == null) throw new KeyNotFoundException("Review not found.");

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
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