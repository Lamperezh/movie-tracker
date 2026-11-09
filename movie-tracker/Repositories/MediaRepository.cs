using Microsoft.EntityFrameworkCore;
using movie_tracker.Data;
using movie_tracker.Models;

namespace movie_tracker.Repositories;

public class MediaRepository : IMediaRepository
{
    private readonly AppDbContext _context;

    public MediaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Media>> GetAllAsync()
    {
        return await _context.Media.ToListAsync();
    }

    public async Task<Media?> GetByIdAsync(int id)
    {
        return await _context.Media.FindAsync(id);
    }

    public async Task<Media?> GetByTmdbIdAsync(int tmdbId, MediaType type)
    {
        return await _context.Media.FirstOrDefaultAsync(m => m.TmdbId == tmdbId && m.Type == type);
    }

    public async Task AddAsync(Media media)
    {
        await _context.Media.AddAsync(media);
    }

    public void Update(Media media)
    {
        _context.Media.Update(media);
    }

    public void Delete(Media media)
    {
        _context.Media.Remove(media);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}