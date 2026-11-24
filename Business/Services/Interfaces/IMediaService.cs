using Business.DTOs.Media;

namespace Business.Services;

public interface IMediaService
{
    Task<IEnumerable<MediaDto>> GetAllMediaAsync();
    Task<MediaDto?> GetMediaByIdAsync(int id);
    Task<MediaDto> CreateMediaAsync(CreateMediaDto dto);
    Task<bool> UpdateMediaAsync(int id, UpdateMediaDto dto);
    Task<bool> DeleteMediaAsync(int id);
}