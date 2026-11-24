using Business.Enums;
using Business.Models;
using Business.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence.Repositories;

public class WatchlistItemRepository : IWatchlistItemRepository
{
    private readonly AppDbContext _context;

    public WatchlistItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<WatchlistItem>> GetAllAsync(
        int userId,
        MediaType? type,
        WatchStatus? status)
    {
        var query = _context.WatchlistItems
            .Where(x => x.UserId == userId);

        if (type.HasValue)
            query = query.Where(x => x.Type == type.Value);

        if (status.HasValue)
            query = query.Where(x => x.Status == status.Value);

        return await query
            .OrderByDescending(x => x.AddedAt)
            .ToListAsync();
    }

    public async Task<WatchlistItem?> GetByIdAsync(int id, int userId)
    {
        return await _context.WatchlistItems
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
    }

    public async Task<bool> UserExistsAsync(int userId)
    {
        return await _context.Set<User>()
            .AnyAsync(x => x.Id == userId);
    }

    public async Task<bool> ExistsAsync(
        int userId,
        int tmdbId,
        MediaType type,
        int? excludeId = null)
    {
        return await _context.WatchlistItems
            .AnyAsync(x =>
                x.UserId == userId &&
                x.TmdbId == tmdbId &&
                x.Type == type &&
                (!excludeId.HasValue || x.Id != excludeId.Value));
    }

    public async Task AddAsync(WatchlistItem item)
    {
        await _context.WatchlistItems.AddAsync(item);
    }

    public void Delete(WatchlistItem item)
    {
        _context.WatchlistItems.Remove(item);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}