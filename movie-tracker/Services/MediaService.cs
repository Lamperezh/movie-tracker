using movie_tracker.DTOs;
using movie_tracker.Models;
using movie_tracker.Repositories;

namespace movie_tracker.Services;

public class MediaService : IMediaService
{
    private readonly IMediaRepository _repository;

    public MediaService(IMediaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<MediaDto>> GetAllMediaAsync()
    {
        var mediaList = await _repository.GetAllAsync();
        return mediaList.Select(m => new MediaDto
        {
            Id = m.Id,
            TmdbId = m.TmdbId,
            Type = (int)m.Type,
            OstLinks = m.OstLinks
        });
    }

    public async Task<MediaDto?> GetMediaByIdAsync(int id)
    {
        var m = await _repository.GetByIdAsync(id);
        if (m == null) return null;

        return new MediaDto
        {
            Id = m.Id,
            TmdbId = m.TmdbId,
            Type = (int)m.Type,
            OstLinks = m.OstLinks
        };
    }

    public async Task<MediaDto> CreateMediaAsync(CreateMediaDto dto)
    {
        var existing = await _repository.GetByTmdbIdAsync(dto.TmdbId, (MediaType)dto.Type);
        if (existing != null)
        {
            return new MediaDto { Id = existing.Id, TmdbId = existing.TmdbId, Type = (int)existing.Type, OstLinks = existing.OstLinks };
        }

        var media = new Media
        {
            TmdbId = dto.TmdbId,
            Type = (MediaType)dto.Type
        };

        await _repository.AddAsync(media);
        await _repository.SaveChangesAsync();

        return new MediaDto { Id = media.Id, TmdbId = media.TmdbId, Type = (int)media.Type, OstLinks = media.OstLinks };
    }

    public async Task<bool> UpdateMediaAsync(int id, UpdateMediaDto dto)
    {
        var media = await _repository.GetByIdAsync(id);
        if (media == null) return false;

        media.OstLinks = dto.OstLinks;
        media.UpdatedAt = DateTime.UtcNow;

        _repository.Update(media);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteMediaAsync(int id)
    {
        var media = await _repository.GetByIdAsync(id);
        if (media == null) return false;

        _repository.Delete(media);
        await _repository.SaveChangesAsync();
        return true;
    }
}