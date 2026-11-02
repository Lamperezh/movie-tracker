using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using movie_tracker.Data;
using movie_tracker.DTOs.WatchlistItem;
using movie_tracker.Models;

namespace movie_tracker.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WatchlistItemsController : ControllerBase
{
    private readonly AppDbContext _context;

    public WatchlistItemsController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WatchlistItemDto>>> GetAll(
        [FromQuery] int userId,
        [FromQuery] MediaType? type,
        [FromQuery] WatchStatus? status)
    {
        if (userId <= 0)
            return BadRequest("User ID must be greater than 0.");

        var query = _context.WatchlistItems
            .Where(x => x.UserId == userId);

        if (type.HasValue)
        {
            if (!Enum.IsDefined(typeof(MediaType), type.Value))
                return BadRequest("Invalid media type.");

            query = query.Where(x => x.Type == type.Value);
        }

        if (status.HasValue)
        {
            if (!Enum.IsDefined(typeof(WatchStatus), status.Value))
                return BadRequest("Invalid watch status.");

            query = query.Where(x => x.Status == status.Value);
        }

        var items = await query
            .OrderByDescending(x => x.AddedAt)
            .Select(x => ToDto(x))
            .ToListAsync();

        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<WatchlistItemDto>> GetById(
        int id,
        [FromQuery] int userId)
    {
        if (id <= 0 || userId <= 0)
            return BadRequest("ID and User ID must be greater than 0.");

        var item = await _context.WatchlistItems
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

        if (item == null)
            return NotFound();

        return Ok(ToDto(item));
    }

    [HttpPost]
    public async Task<ActionResult<WatchlistItemDto>> Create(
        CreateWatchlistItemDto dto,
        [FromQuery] int userId)
    {
        if (userId <= 0)
            return BadRequest("User ID must be greater than 0.");

        if (dto.TmdbId <= 0)
            return BadRequest("TMDB ID must be greater than 0.");

        if (!Enum.IsDefined(typeof(MediaType), dto.Type))
            return BadRequest("Invalid media type.");

        if (!Enum.IsDefined(typeof(WatchStatus), dto.Status))
            return BadRequest("Invalid watch status.");

        if (string.IsNullOrWhiteSpace(dto.Title))
            return BadRequest("Title is required.");

        var userExists = await _context.Set<User>()
            .AnyAsync(x => x.Id == userId);

        if (!userExists)
            return NotFound("User not found.");

        var alreadyExists = await _context.WatchlistItems
            .AnyAsync(x =>
                x.UserId == userId &&
                x.TmdbId == dto.TmdbId &&
                x.Type == dto.Type);

        if (alreadyExists)
            return Conflict("This media is already in the user's watchlist.");

        var now = DateTime.UtcNow;

        var item = new WatchlistItem
        {
            UserId = userId,
            TmdbId = dto.TmdbId,
            Type = dto.Type,
            Title = dto.Title.Trim(),
            PosterPath = dto.PosterPath,
            Status = dto.Status,
            AddedAt = now
        };

        _context.WatchlistItems.Add(item);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = item.Id, userId = item.UserId },
            ToDto(item));
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult<WatchlistItemDto>> Update(
        int id,
        UpdateWatchlistItemDto dto,
        [FromQuery] int userId)
    {
        if (id <= 0 || userId <= 0)
            return BadRequest("ID and User ID must be greater than 0.");

        if (dto.TmdbId <= 0)
            return BadRequest("TMDB ID must be greater than 0.");

        if (!Enum.IsDefined(typeof(MediaType), dto.Type))
            return BadRequest("Invalid media type.");

        if (!Enum.IsDefined(typeof(WatchStatus), dto.Status))
            return BadRequest("Invalid watch status.");

        if (string.IsNullOrWhiteSpace(dto.Title))
            return BadRequest("Title is required.");

        var item = await _context.WatchlistItems
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

        if (item == null)
            return NotFound();

        var duplicateExists = await _context.WatchlistItems
            .AnyAsync(x =>
                x.Id != id &&
                x.UserId == userId &&
                x.TmdbId == dto.TmdbId &&
                x.Type == dto.Type);

        if (duplicateExists)
            return Conflict("This media is already in the user's watchlist.");

        item.TmdbId = dto.TmdbId;
        item.Type = dto.Type;
        item.Title = dto.Title.Trim();
        item.PosterPath = dto.PosterPath;
        item.Status = dto.Status;
        item.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(ToDto(item));
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        [FromQuery] int userId)
    {
        if (id <= 0 || userId <= 0)
            return BadRequest("ID and User ID must be greater than 0.");

        var item = await _context.WatchlistItems
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

        if (item == null)
            return NotFound();

        _context.WatchlistItems.Remove(item);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static WatchlistItemDto ToDto(WatchlistItem item)
    {
        return new WatchlistItemDto
        {
            Id = item.Id,
            UserId = item.UserId,
            TmdbId = item.TmdbId,
            Type = item.Type,
            Title = item.Title,
            PosterPath = item.PosterPath,
            Status = item.Status,
            AddedAt = item.AddedAt,
            UpdatedAt = item.UpdatedAt
        };
    }
}