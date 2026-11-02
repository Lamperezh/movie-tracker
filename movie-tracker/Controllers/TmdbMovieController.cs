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
        if (filters.Page < 1 || filters.Page > 500)
            return BadRequest("Page must be between 1 and 500.");

        if (filters.PrimaryReleaseYear is < 1874 or > 2100)
            return BadRequest("Invalid release year.");

        if (filters.VoteAverageGte is < 0 or > 10 ||
            filters.VoteAverageLte is < 0 or > 10)
        {
            return BadRequest("Vote average must be between 0 and 10.");
        }

        if (filters.VoteAverageGte.HasValue &&
            filters.VoteAverageLte.HasValue &&
            filters.VoteAverageGte > filters.VoteAverageLte)
        {
            return BadRequest("Minimum vote average cannot exceed maximum.");
        }

        if (filters.VoteCountGte is < 0 ||
            filters.VoteCountLte is < 0 ||
            filters.WithRuntimeGte is < 0 ||
            filters.WithRuntimeLte is < 0)
        {
            return BadRequest("Vote counts and runtime cannot be negative.");
        }

        if (filters.ReleaseDateGte.HasValue &&
            filters.ReleaseDateLte.HasValue &&
            filters.ReleaseDateGte > filters.ReleaseDateLte)
        {
            return BadRequest("Start date cannot be later than end date.");
        }

        var allowedSortOptions = new[]
        {
            "popularity.asc",
            "popularity.desc",
            "primary_release_date.asc",
            "primary_release_date.desc",
            "revenue.asc",
            "revenue.desc",
            "original_title.asc",
            "original_title.desc",
            "title.asc",
            "title.desc",
            "vote_average.asc",
            "vote_average.desc",
            "vote_count.asc",
            "vote_count.desc"
        };

        if (filters.SortBy is not null && !allowedSortOptions.Contains(filters.SortBy))
            return BadRequest("Invalid sort option.");

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