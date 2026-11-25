using Business.DTOs.Media;
using Business.Services;
using Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace movie_tracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MediaController : ControllerBase
{
    private readonly IMediaService _mediaService;

    public MediaController(IMediaService mediaService)
    {
        _mediaService = mediaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MediaDto>>> GetMedia()
    {
        return Ok(await _mediaService.GetAllMediaAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MediaDto>> GetMedia(int id)
    {
        var result = await _mediaService.GetMediaByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet("tmdb/{tmdbId}")]
    public async Task<ActionResult<MediaDto>> GetMediaByTmdbId(int tmdbId, [FromQuery] int type)
    {
        var result = await _mediaService.GetMediaByTmdbIdAsync(tmdbId, type);

        if (result == null) return NotFound();

        return Ok(result);
    }
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<MediaDto>> CreateMedia(CreateMediaDto dto)
    {
        var result = await _mediaService.CreateMediaAsync(dto);
        return Ok(result);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMedia(int id, UpdateMediaDto dto)
    {
        var result = await _mediaService.UpdateMediaAsync(id, dto);
        if (!result) return NotFound();
        return NoContent();
    }

    [Authorize(Roles = "Administrator")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMedia(int id)
    {
        var result = await _mediaService.DeleteMediaAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}