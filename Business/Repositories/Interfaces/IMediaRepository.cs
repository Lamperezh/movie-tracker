using Business.Enums;
using Business.Models;

namespace Business.Repositories;

public interface IMediaRepository
{
    Task<IEnumerable<Media>> GetAllAsync();
    Task<Media?> GetByIdAsync(int id);
    Task<Media?> GetByTmdbIdAsync(int tmdbId, MediaType type);
    Task AddAsync(Media media);
    void Update(Media media);
    void Delete(Media media);
    Task SaveChangesAsync();
}