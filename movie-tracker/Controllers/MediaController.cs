using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using movie_tracker.Data;
using movie_tracker.DTOs.Media;
using movie_tracker.Models;

namespace movie_tracker.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MediaController : ControllerBase
{
    private readonly AppDbContext _context;

    public MediaController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MediaDto>>> GetAll()
    {
        var mediaItems = await _context.Media.ToListAsync();

        var mediaDtos = mediaItems.Select(ToDto);

        return Ok(mediaDtos);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<MediaDto>> GetById(int id)
    {
        if (id <= 0)
            return BadRequest("ID must be greater than 0.");

        var media = await _context.Media.FindAsync(id);

        if (media == null)
            return NotFound();

        return Ok(ToDto(media));
    }
    
    [HttpPost]
    public async Task<ActionResult<MediaDto>> Create(CreateMediaDto dto)
    {
        if (dto.TmdbId <= 0)
            return BadRequest("TMDB ID must be greater than 0.");

        if (!Enum.IsDefined(typeof(MediaType), dto.Type))
            return BadRequest("Invalid media type.");

        var media = new Media
        {
            TmdbId = dto.TmdbId,
            Type = dto.Type,
            OstLinks = dto.OstLinks ?? new List<string>(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Media.Add(media);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = media.Id },
            ToDto(media));
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult<MediaDto>> Update(
        int id,
        UpdateMediaDto dto)
    {
        if (id <= 0)
            return BadRequest("ID must be greater than 0.");

        if (dto.TmdbId <= 0)
            return BadRequest("TMDB ID must be greater than 0.");

        if (!Enum.IsDefined(typeof(MediaType), dto.Type))
            return BadRequest("Invalid media type.");

        var media = await _context.Media.FindAsync(id);

        if (media == null)
            return NotFound();

        media.TmdbId = dto.TmdbId;
        media.Type = dto.Type;
        media.OstLinks = dto.OstLinks ?? new List<string>();
        media.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(ToDto(media));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
            return BadRequest("ID must be greater than 0.");

        var media = await _context.Media.FindAsync(id);

        if (media == null)
            return NotFound();

        _context.Media.Remove(media);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static MediaDto ToDto(Media media)
    {
        return new MediaDto
        {
            Id = media.Id,
            TmdbId = media.TmdbId,
            Type = media.Type,
            OstLinks = media.OstLinks,
            CreatedAt = media.CreatedAt,
            UpdatedAt = media.UpdatedAt
        };
    }
}