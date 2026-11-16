using movie_tracker.DTOs.WatchlistItem;
using movie_tracker.Enums;

namespace movie_tracker.Services;

public interface IWatchlistItemService
{
    Task<List<WatchlistItemDto>> GetAllAsync(
        int userId,
        MediaType? type,
        WatchStatus? status);

    Task<WatchlistItemDto?> GetByIdAsync(int id, int userId);

    Task<WatchlistItemDto?> CreateAsync(
        CreateWatchlistItemDto dto,
        int userId);

    Task<WatchlistItemDto?> UpdateAsync(
        int id,
        UpdateWatchlistItemDto dto,
        int userId);

    Task<bool> DeleteAsync(int id, int userId);
}