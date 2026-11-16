using Microsoft.AspNetCore.Mvc;
using movie_tracker.DTOs.Tmdb.TV;
using movie_tracker.Services;

namespace movie_tracker.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TmdbTvController : ControllerBase
{
    private readonly TmdbService _tmdbService;

    public TmdbTvController(TmdbService tmdbService)
    {
        _tmdbService = tmdbService;
    }

    [HttpGet("discover")]
    public async Task<IActionResult> DiscoverTv(
        [FromQuery] TmdbDiscoverTvRequestDto filters)
    {
        try
        {
            var tvShows = await _tmdbService.DiscoverTvAsync(filters);

            if (tvShows is null)
            {
                return StatusCode(
                    StatusCodes.Status502BadGateway,
                    "Invalid response from TMDB.");
            }

            return Ok(tvShows);
        }
        catch (HttpRequestException)
        {
            return StatusCode(
                StatusCodes.Status502BadGateway,
                "Failed to fetch data from TMDB.");
        }
    }
    
    [HttpGet("tv/{tvId:int}")]
    public async Task<IActionResult> GetTvDetails(int tvId)
    {
        if (tvId <= 0)
            return BadRequest("TV ID must be greater than 0.");

        try
        {
            var tv = await _tmdbService.GetTvDetailsAsync(tvId);

            if (tv is null)
            {
                return StatusCode(
                    StatusCodes.Status502BadGateway,
                    "Invalid response from TMDB.");
            }

            return Ok(tv);
        }
        catch (HttpRequestException)
        {
            return StatusCode(
                StatusCodes.Status502BadGateway,
                "Failed to fetch data from TMDB.");
        }
    }
    
    [HttpGet("popular")]
    public async Task<IActionResult> GetPopularTv([FromQuery] int page = 1)
    {
        if (page < 1) 
            return BadRequest("Page must be greater than 0.");

        try
        {
            var movies = await _tmdbService.GetPopularTvAsync(page);

            if (movies is null)
            {
                return StatusCode(
                    StatusCodes.Status502BadGateway,
                    "Invalid response from TMDB.");
            }

            return Ok(movies);
        }
        catch (HttpRequestException)
        {
            return StatusCode(
                StatusCodes.Status502BadGateway,
                "Failed to fetch data from TMDB.");
        }
    }
}