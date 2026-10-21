using Microsoft.AspNetCore.Mvc;
using movie_tracker.Services;

namespace movie_tracker.Controllers;

[Route("api/test-movies")]
[ApiController]
public class TestMovieController : ControllerBase
{
    private readonly TmdbService _tmdbService;

    public TestMovieController(TmdbService tmdbService)
    {
        _tmdbService = tmdbService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPopularMovies([FromQuery] int page = 1)
    {
        if (page < 1)
        {
            return BadRequest("Page must be greater than 0.");
        }

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