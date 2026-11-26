using Business.Enums;
using Business.Models;
using Business.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly AppDbContext _context;

    public ReviewRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Review>> GetAllAsync(int? userId, int? tmdbId, MediaType? type)
    {
        var query = _context.Reviews
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .AsQueryable();

        if (userId.HasValue)
            query = query.Where(x => x.UserId == userId.Value);

        if (tmdbId.HasValue)
            query = query.Where(x => x.TmdbId == tmdbId.Value);

        if (type.HasValue)
            query = query.Where(x => x.Type == type.Value);

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<Review?> GetByIdAsync(int id)
    {
        return await _context.Reviews
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Review?> GetByIdAndUserIdAsync(int id, int userId)
    {
        return await _context.Reviews
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.UserId == userId);
    }

    public async Task<Review?> GetByUserAndMediaAsync(int userId, int tmdbId, MediaType type)
    {
        return await _context.Reviews
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .FirstOrDefaultAsync(x =>
                x.UserId == userId &&
                x.TmdbId == tmdbId &&
                x.Type == type);
    }

    public async Task<CommentReaction?> GetReactionAsync(int reviewId, int userId)
    {
        return await _context.Set<CommentReaction>()
            .FirstOrDefaultAsync(x =>
                x.ReviewId == reviewId &&
                x.UserId == userId);
    }

    public async Task<bool> UserExistsAsync(int userId)
    {
        return await _context.Set<User>()
            .AnyAsync(x => x.Id == userId);
    }

    public async Task<bool> ExistsAsync(int userId, int tmdbId, MediaType type, int? excludeId = null)
    {
        return await _context.Reviews.AnyAsync(x =>
            x.UserId == userId &&
            x.TmdbId == tmdbId &&
            x.Type == type &&
            (!excludeId.HasValue || x.Id != excludeId.Value));
    }

    public async Task AddAsync(Review review)
    {
        await _context.Reviews.AddAsync(review);
    }

    public async Task AddReactionAsync(CommentReaction reaction)
    {
        await _context.Set<CommentReaction>().AddAsync(reaction);
    }

    public void Delete(Review review)
    {
        _context.Reviews.Remove(review);
    }

    public void DeleteReaction(CommentReaction reaction)
    {
        _context.Set<CommentReaction>().Remove(reaction);
    }

    public void DeleteReactions(IEnumerable<CommentReaction> reactions)
    {
        _context.Set<CommentReaction>().RemoveRange(reactions);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}