using System.Security.Claims;
using Business.DTOs.Review;
using Business.Enums;
using Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetAll(
        [FromQuery] int? userId,
        [FromQuery] int? tmdbId,
        [FromQuery] MediaType? type)
    {
        int? currentUserId = TryGetUserId(out var id) ? id : null;

        var reviews = await _reviewsService.GetAllAsync(
            userId,
            tmdbId,
            type,
            currentUserId);

        return Ok(reviews);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReviewDto>> GetById(int id)
    {
        int? currentUserId = TryGetUserId(out var userId)
            ? userId
            : null;

        var review = await _reviewsService.GetByIdAsync(
            id,
            currentUserId);

        return Ok(review);
    }
    
    [Authorize]
    [HttpPost("rating")]
    public async Task<ActionResult<ReviewDto>> CreateRating(
        CreateReviewDto dto)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();

        var result = await _reviewsService.CreateRatingAsync(dto, userId);

        return Ok(result);
    }

    [Authorize]
    [HttpPut("{id:int}/rating")]
    public async Task<ActionResult<ReviewDto>> UpdateRating(
        int id,
        UpdateReviewDto dto)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();

        var result = await _reviewsService.UpdateRatingAsync(
            id,
            dto,
            userId);

        return Ok(result);
    }

    [Authorize]
    [HttpDelete("{id:int}/rating")]
    public async Task<IActionResult> DeleteRating(int id)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();

        await _reviewsService.DeleteRatingAsync(id, userId);

        return NoContent();
    }
    
    [Authorize]
    [HttpPost("comment")]
    public async Task<ActionResult<ReviewDto>> CreateComment(
        CreateCommentDto dto)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();

        var result = await _reviewsService.CreateCommentAsync(dto, userId);

        return Ok(result);
    }

    [Authorize]
    [HttpPut("{id:int}/comment")]
    public async Task<ActionResult<ReviewDto>> UpdateComment(
        int id,
        UpdateCommentDto dto)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();

        var result = await _reviewsService.UpdateCommentAsync(
            id,
            dto,
            userId);

        return Ok(result);
    }

    [Authorize]
    [HttpDelete("{id:int}/comment")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();

        await _reviewsService.DeleteCommentAsync(id, userId);

        return NoContent();
    }
    
    [Authorize]
    [HttpPost("{id:int}/reaction")]
    public async Task<ActionResult<ReviewDto>> SetReaction(
        int id,
        SetCommentReactionDto dto)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();

        var result = await _reviewsService.SetReactionAsync(
            id,
            dto,
            userId);

        return Ok(result);
    }

    [Authorize]
    [HttpDelete("{id:int}/reaction")]
    public async Task<IActionResult> DeleteReaction(int id)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();

        await _reviewsService.DeleteReactionAsync(id, userId);

        return NoContent();
    }
    
    private bool TryGetUserId(out int userId)
    {
        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? User.FindFirstValue("sub");

        return int.TryParse(claim, out userId) && userId > 0;
    }
}