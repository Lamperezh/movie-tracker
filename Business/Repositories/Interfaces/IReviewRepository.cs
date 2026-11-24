using Business.Enums;
using Business.Models;

namespace Business.Repositories;

public interface IReviewRepository
{
    Task<List<Review>> GetAllAsync(
        int? userId,
        int? tmdbId,
        MediaType? type);

    Task<Review?> GetByIdAsync(int id);

    Task<Review?> GetByIdAndUserIdAsync(int id, int userId);

    Task<bool> UserExistsAsync(int userId);

    Task<bool> ExistsAsync(
        int userId,
        int tmdbId,
        MediaType type,
        int? excludeId = null);

    Task AddAsync(Review review);

    void Delete(Review review);

    Task SaveChangesAsync();
}