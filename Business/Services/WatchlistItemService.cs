using Business.DTOs.WatchlistItem;
using Business.Enums;
using Business.Models;
using Business.Repositories;
using Business.Services.Interfaces;

namespace Business.Services;

public class WatchlistItemService : IWatchlistItemService
{
    private readonly IWatchlistItemRepository _repository;

    public WatchlistItemService(IWatchlistItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<WatchlistItemDto>> GetAllAsync(
        int userId,
        MediaType? type,
        WatchStatus? status)
    {
        var items = await _repository.GetAllAsync(userId, type, status);

        return items.Select(ToDto).ToList();
    }

    public async Task<WatchlistItemDto?> GetByIdAsync(int id, int userId)
    {
        var item = await _repository.GetByIdAsync(id, userId);

        return item == null ? null : ToDto(item);
    }

    public async Task<WatchlistItemDto?> CreateAsync(
        CreateWatchlistItemDto dto,
        int userId)
    {
        var userExists = await _repository.UserExistsAsync(userId);

        if (!userExists)
            return null;

        var alreadyExists = await _repository.ExistsAsync(
            userId, dto.TmdbId, dto.Type);

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

        await _repository.AddAsync(item);
        await _repository.SaveChangesAsync();

        return ToDto(item);
    }

    public async Task<WatchlistItemDto?> UpdateAsync(
        int id,
        UpdateWatchlistItemDto dto,
        int userId)
    {
        var item = await _repository.GetByIdAsync(id, userId);

        if (item == null)
            return null;

        var duplicateExists = await _repository.ExistsAsync(
            userId, dto.TmdbId, dto.Type, id);

        if (duplicateExists)
            return null;

        item.TmdbId = dto.TmdbId;
        item.Type = dto.Type;
        item.Title = dto.Title.Trim();
        item.PosterPath = dto.PosterPath;
        item.Status = dto.Status;
        item.UpdatedAt = DateTime.UtcNow;

        await _repository.SaveChangesAsync();

        return ToDto(item);
    }

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        var item = await _repository.GetByIdAsync(id, userId);

        if (item == null)
            return false;

        _repository.Delete(item);
        await _repository.SaveChangesAsync();

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