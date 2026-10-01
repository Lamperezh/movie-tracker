using Microsoft.AspNetCore.Mvc;
using movie_tracker.DTOs.WatchlistItem;
using movie_tracker.Models;

namespace movie_tracker.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WatchlistController : ControllerBase
{
    private static readonly List<WatchlistItem> WatchlistItems =
    [
        new WatchlistItem
        {
            Id = 1,
            UserId = 1,
            MovieId = 1,
            Status = WatchStatus.Completed
        },
        new WatchlistItem
        {
            Id = 2,
            UserId = 1,
            MovieId = 2,
            Status = WatchStatus.Watching
        },
        new WatchlistItem
        {
            Id = 3,
            UserId = 2,
            MovieId = 1,
            Status = WatchStatus.Planned
        }
    ];
    
    [HttpGet("user/{userId}")]
    public ActionResult<IEnumerable<WatchlistItemDto>> GetUserWatchlist(int userId)
    {
        var items = WatchlistItems
            .Where(item => item.UserId == userId)
            .Select(item => new WatchlistItemDto
            {
                Id = item.Id,
                UserId = item.UserId,
                MovieId = item.MovieId,
                Status = item.Status
            });

        return Ok(items);
    }
    
    [HttpPut("{id}")]
    public ActionResult<WatchlistItemDto> UpdateStatus(
        int id,
        UpdateWatchlistItemDto dto)
    {
        var item = WatchlistItems.FirstOrDefault(item => item.Id == id);

        if (item == null)
        {
            return NotFound();
        }

        item.Status = dto.Status;

        var result = new WatchlistItemDto
        {
            Id = item.Id,
            UserId = item.UserId,
            MovieId = item.MovieId,
            Status = item.Status
        };

        return Ok(result);
    }
    
    [HttpGet("movie/{movieId}/count")]
    public ActionResult<int> GetMovieCount(int movieId)
    {
        var count = WatchlistItems.Count(item => item.MovieId == movieId);

        return Ok(count);
    }
}