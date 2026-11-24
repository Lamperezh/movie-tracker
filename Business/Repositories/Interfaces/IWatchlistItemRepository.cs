using Business.Enums;
using Business.Models;

namespace Business.Repositories;

public interface IWatchlistItemRepository
{
    Task<List<WatchlistItem>> GetAllAsync(
        int userId,
        MediaType? type,
        WatchStatus? status);

    Task<WatchlistItem?> GetByIdAsync(int id, int userId);

    Task<bool> UserExistsAsync(int userId);

    Task<bool> ExistsAsync(
        int userId,
        int tmdbId,
        MediaType type,
        int? excludeId = null);

    Task AddAsync(WatchlistItem item);

    void Delete(WatchlistItem item);

    Task SaveChangesAsync();
}