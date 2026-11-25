using Business.DTOs.Review;
using Business.Enums;

namespace Business.Services.Interfaces;

public interface IReviewsService
{
    Task<IEnumerable<ReviewDto>> GetAllAsync(int? userId, int? tmdbId, MediaType? type);
    Task<ReviewDto> GetByIdAsync(int id);
    Task<ReviewDto> CreateAsync(CreateReviewDto dto, int userId);
    Task<ReviewDto> UpdateAsync(int id, UpdateReviewDto dto, int userId);
    Task DeleteAsync(int id, int userId);
}