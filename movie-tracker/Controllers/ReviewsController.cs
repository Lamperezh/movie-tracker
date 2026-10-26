using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using movie_tracker.Data;
using movie_tracker.DTOs.Review;
using movie_tracker.Models;

namespace movie_tracker.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReviewsController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetAll(
        [FromQuery] int? userId,
        [FromQuery] int? tmdbId,
        [FromQuery] MediaType? type)
    {
        var query = _context.Reviews.AsQueryable();

        if (userId.HasValue)
        {
            if (userId.Value <= 0)
                return BadRequest("User ID must be greater than 0.");

            query = query.Where(x => x.UserId == userId.Value);
        }

        if (tmdbId.HasValue)
        {
            if (tmdbId.Value <= 0)
                return BadRequest("TMDB ID must be greater than 0.");

            query = query.Where(x => x.TmdbId == tmdbId.Value);
        }

        if (type.HasValue)
        {
            if (!Enum.IsDefined(typeof(MediaType), type.Value))
                return BadRequest("Invalid media type.");

            query = query.Where(x => x.Type == type.Value);
        }

        var reviews = await query
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => ToDto(x))
            .ToListAsync();

        return Ok(reviews);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReviewDto>> GetById(int id)
    {
        if (id <= 0)
            return BadRequest("Review ID must be greater than 0.");

        var review = await _context.Reviews.FindAsync(id);

        if (review == null)
            return NotFound();

        return Ok(ToDto(review));
    }
    
    [HttpPost]
    public async Task<ActionResult<ReviewDto>> Create(
        CreateReviewDto dto,
        [FromQuery] int userId)
    {
        if (userId <= 0)
            return BadRequest("User ID must be greater than 0.");

        if (dto.TmdbId <= 0)
            return BadRequest("TMDB ID must be greater than 0.");

        if (!Enum.IsDefined(typeof(MediaType), dto.Type))
            return BadRequest("Invalid media type.");

        if (dto.Rating < 1 || dto.Rating > 10)
            return BadRequest("Rating must be between 1 and 10.");

        var userExists = await _context.Set<User>()
            .AnyAsync(x => x.Id == userId);

        if (!userExists)
            return NotFound("User not found.");

        var alreadyExists = await _context.Reviews.AnyAsync(x =>
            x.UserId == userId &&
            x.TmdbId == dto.TmdbId &&
            x.Type == dto.Type);

        if (alreadyExists)
            return Conflict("You have already reviewed this media.");

        var review = new Review
        {
            UserId = userId,
            TmdbId = dto.TmdbId,
            Type = dto.Type,
            Rating = dto.Rating,
            Comment = dto.Comment?.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = review.Id },
            ToDto(review));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ReviewDto>> Update(
        int id,
        UpdateReviewDto dto,
        [FromQuery] int userId)
    {
        if (id <= 0 || userId <= 0)
            return BadRequest("Review ID and User ID must be greater than 0.");

        if (dto.TmdbId <= 0)
            return BadRequest("TMDB ID must be greater than 0.");

        if (!Enum.IsDefined(typeof(MediaType), dto.Type))
            return BadRequest("Invalid media type.");

        if (dto.Rating < 1 || dto.Rating > 10)
            return BadRequest("Rating must be between 1 and 10.");

        var review = await _context.Reviews
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

        if (review == null)
            return NotFound();

        var duplicateExists = await _context.Reviews.AnyAsync(x =>
            x.Id != id &&
            x.UserId == userId &&
            x.TmdbId == dto.TmdbId &&
            x.Type == dto.Type);

        if (duplicateExists)
            return Conflict("You have already reviewed this media.");

        review.TmdbId = dto.TmdbId;
        review.Type = dto.Type;
        review.Rating = dto.Rating;
        review.Comment = dto.Comment?.Trim();

        await _context.SaveChangesAsync();

        return Ok(ToDto(review));
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        [FromQuery] int userId)
    {
        if (id <= 0 || userId <= 0)
            return BadRequest("Review ID and User ID must be greater than 0.");

        var review = await _context.Reviews
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

        if (review == null)
            return NotFound();

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static ReviewDto ToDto(Review review)
    {
        return new ReviewDto
        {
            Id = review.Id,
            UserId = review.UserId,
            TmdbId = review.TmdbId,
            Type = review.Type,
            Rating = review.Rating,
            Comment = review.Comment,
            CreatedAt = review.CreatedAt
        };
    }
}