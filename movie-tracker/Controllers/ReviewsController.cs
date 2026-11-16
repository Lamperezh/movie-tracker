using Microsoft.AspNetCore.Mvc;
using movie_tracker.DTOs.Review;
using movie_tracker.Enums;
using movie_tracker.Services;

namespace movie_tracker.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewsController : ControllerBase
{
    private readonly IReviewsService _reviewsService;

    public ReviewsController(IReviewsService reviewsService)
    {
        _reviewsService = reviewsService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetAll([FromQuery] int? userId, [FromQuery] int? tmdbId, [FromQuery] MediaType? type)
    {
        return Ok(await _reviewsService.GetAllAsync(userId, tmdbId, type));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReviewDto>> GetById(int id)
    {
        return Ok(await _reviewsService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult<ReviewDto>> Create(CreateReviewDto dto, [FromQuery] int userId)
    {
        var result = await _reviewsService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ReviewDto>> Update(int id, UpdateReviewDto dto, [FromQuery] int userId)
    {
        return Ok(await _reviewsService.UpdateAsync(id, dto, userId));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, [FromQuery] int userId)
    {
        await _reviewsService.DeleteAsync(id, userId);
        return NoContent();
    }
}