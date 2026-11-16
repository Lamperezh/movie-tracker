using Microsoft.AspNetCore.Mvc;
using movie_tracker.DTOs.Tmdb.Movie;
using movie_tracker.Services;

namespace movie_tracker.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TmdbMovieController : ControllerBase
{
    private readonly TmdbService _tmdbService;

    public TmdbMovieController(TmdbService tmdbService)
    {
        _tmdbService = tmdbService;
    }

    [HttpGet("discover")]
    public async Task<IActionResult> DiscoverMovies(
        [FromQuery] TmdbDiscoverMoviesRequestDto filters)
    {
        try
        {
            var movies = await _tmdbService.DiscoverMoviesAsync(filters);

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
    
    [HttpGet("{movieId:int}")]
    public async Task<IActionResult> GetMovieDetails(int movieId)
    {
        if (movieId <= 0)
            return BadRequest("Movie ID must be greater than 0.");

        try
        {
            var movie = await _tmdbService.GetMovieDetailsAsync(movieId);

            if (movie is null)
            {
                return StatusCode(
                    StatusCodes.Status502BadGateway,
                    "Invalid response from TMDB.");
            }

            return Ok(movie);
        }
        catch (HttpRequestException)
        {
            return StatusCode(
                StatusCodes.Status502BadGateway,
                "Failed to fetch data from TMDB.");
        }
    }
    
    [HttpGet("popular")]
    public async Task<IActionResult> GetPopularMovies([FromQuery] int page = 1)
    {
        if (page < 1) 
            return BadRequest("Page must be greater than 0.");

        try
        {
            var movies = await _tmdbService.GetPopularMoviesAsync(page);

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