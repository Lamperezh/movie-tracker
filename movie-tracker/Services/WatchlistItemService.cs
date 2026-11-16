using Microsoft.EntityFrameworkCore;
using movie_tracker.Data;
using movie_tracker.DTOs.WatchlistItem;
using movie_tracker.Enums;
using movie_tracker.Models;

namespace movie_tracker.Services;

public class WatchlistItemService : IWatchlistItemService
{
    private readonly AppDbContext _context;

    public WatchlistItemService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<WatchlistItemDto>> GetAllAsync(
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
            .Select(x => ToDto(x))
            .ToListAsync();
    }

    public async Task<WatchlistItemDto?> GetByIdAsync(int id, int userId)
    {
        var item = await _context.WatchlistItems
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

        return item == null ? null : ToDto(item);
    }

    public async Task<WatchlistItemDto?> CreateAsync(
        CreateWatchlistItemDto dto,
        int userId)
    {
        var userExists = await _context.Set<User>()
            .AnyAsync(x => x.Id == userId);

        if (!userExists)
            return null;

        var alreadyExists = await _context.WatchlistItems
            .AnyAsync(x =>
                x.UserId == userId &&
                x.TmdbId == dto.TmdbId &&
                x.Type == dto.Type);

        if (alreadyExists)
            return null;

        var item = new WatchlistItem
        {
            UserId = userId,
            TmdbId = dto.TmdbId,
            Type = dto.Type,
            Title = dto.Title.Trim(),
            PosterPath = dto.PosterPath,
            Status = dto.Status,
            AddedAt = DateTime.UtcNow
        };

        _context.WatchlistItems.Add(item);
        await _context.SaveChangesAsync();

        return ToDto(item);
    }

    public async Task<WatchlistItemDto?> UpdateAsync(
        int id,
        UpdateWatchlistItemDto dto,
        int userId)
    {
        var item = await _context.WatchlistItems
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

        if (item == null)
            return null;

        var duplicateExists = await _context.WatchlistItems
            .AnyAsync(x =>
                x.Id != id &&
                x.UserId == userId &&
                x.TmdbId == dto.TmdbId &&
                x.Type == dto.Type);

        if (duplicateExists)
            return null;

        item.TmdbId = dto.TmdbId;
        item.Type = dto.Type;
        item.Title = dto.Title.Trim();
        item.PosterPath = dto.PosterPath;
        item.Status = dto.Status;
        item.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return ToDto(item);
    }

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        var item = await _context.WatchlistItems
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

        if (item == null)
            return false;

        _context.WatchlistItems.Remove(item);
        await _context.SaveChangesAsync();

        return true;
    }

    private static WatchlistItemDto ToDto(WatchlistItem item)
    {
        return new WatchlistItemDto
        {
            Id = item.Id,
            UserId = item.UserId,
            TmdbId = item.TmdbId,
            Type = item.Type,
            Title = item.Title,
            PosterPath = item.PosterPath,
            Status = item.Status,
            AddedAt = item.AddedAt,
            UpdatedAt = item.UpdatedAt
        };
    }
}
