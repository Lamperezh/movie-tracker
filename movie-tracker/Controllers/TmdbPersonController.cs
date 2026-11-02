using Microsoft.AspNetCore.Mvc;
using movie_tracker.Services;

namespace movie_tracker.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TmdbPersonController : ControllerBase
{
    private readonly TmdbService _tmdbService;

    public TmdbPersonController(TmdbService tmdbService)
    {
        _tmdbService = tmdbService;
    }

    [HttpGet("{personId:int}")]
    public async Task<IActionResult> GetPersonDetails(int personId)
    {
        if (personId <= 0)
            return BadRequest("Actor ID must be greater than 0.");

        try
        {
            var actor = await _tmdbService.GetPersonDetailsAsync(personId);

            if (actor is null)
            {
                return StatusCode(
                    StatusCodes.Status502BadGateway,
                    "Invalid response from TMDB.");
            }

            return Ok(actor);
        }
        catch (HttpRequestException)
        {
            return StatusCode(
                StatusCodes.Status502BadGateway,
                "Failed to fetch data from TMDB.");
        }
    }
}