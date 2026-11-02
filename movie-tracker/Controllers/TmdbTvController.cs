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
        if (filters.Page < 1 || filters.Page > 500)
            return BadRequest("Page must be between 1 and 500.");

        if (filters.FirstAirDateYear is < 1874 or > 2100)
            return BadRequest("Invalid first air date year.");

        if (filters.VoteAverageGte is < 0 or > 10 ||
            filters.VoteAverageLte is < 0 or > 10)
        {
            return BadRequest("Vote average must be between 0 and 10.");
        }

        if (filters.VoteAverageGte.HasValue &&
            filters.VoteAverageLte.HasValue &&
            filters.VoteAverageGte > filters.VoteAverageLte)
        {
            return BadRequest(
                "Minimum vote average cannot exceed maximum.");
        }

        if (filters.VoteCountGte is < 0 ||
            filters.VoteCountLte is < 0 ||
            filters.WithRuntimeGte is < 0 ||
            filters.WithRuntimeLte is < 0)
        {
            return BadRequest(
                "Vote counts and runtime cannot be negative.");
        }

        if (filters.FirstAirDateGte.HasValue &&
            filters.FirstAirDateLte.HasValue &&
            filters.FirstAirDateGte > filters.FirstAirDateLte)
        {
            return BadRequest(
                "First air date start cannot be later than end date.");
        }

        if (filters.AirDateGte.HasValue &&
            filters.AirDateLte.HasValue &&
            filters.AirDateGte > filters.AirDateLte)
        {
            return BadRequest(
                "Air date start cannot be later than end date.");
        }

        var allowedSortOptions = new[]
        {
            "popularity.asc",
            "popularity.desc",
            "first_air_date.asc",
            "first_air_date.desc",
            "vote_average.asc",
            "vote_average.desc",
            "vote_count.asc",
            "vote_count.desc",
            "original_name.asc",
            "original_name.desc",
            "name.asc",
            "name.desc"
        };

        if (filters.SortBy is not null &&
            !allowedSortOptions.Contains(filters.SortBy))
        {
            return BadRequest("Invalid sort option.");
        }

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