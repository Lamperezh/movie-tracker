using movie_tracker.DTOs.Media;

namespace movie_tracker.Services;

public interface IMediaService
{
    Task<IEnumerable<MediaDto>> GetAllMediaAsync();
    Task<MediaDto?> GetMediaByIdAsync(int id);
    Task<MediaDto> CreateMediaAsync(CreateMediaDto dto);
    Task<bool> UpdateMediaAsync(int id, UpdateMediaDto dto);
    Task<bool> DeleteMediaAsync(int id);
}