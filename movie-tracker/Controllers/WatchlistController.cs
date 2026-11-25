using Business.DTOs.WatchlistItem;
using Business.Enums;
using Business.Services;
using Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace movie_tracker.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WatchlistItemsController : ControllerBase
{
    private readonly IWatchlistItemService _service;

    public WatchlistItemsController(IWatchlistItemService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WatchlistItemDto>>> GetAll(
        [FromQuery] int userId,
        [FromQuery] MediaType? type,
        [FromQuery] WatchStatus? status)
    {
        if (userId <= 0)
            return BadRequest("User ID must be greater than 0.");

        if (type.HasValue && !Enum.IsDefined(typeof(MediaType), type.Value))
            return BadRequest("Invalid media type.");

        if (status.HasValue && !Enum.IsDefined(typeof(WatchStatus), status.Value))
            return BadRequest("Invalid watch status.");

        var items = await _service.GetAllAsync(userId, type, status);

        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<WatchlistItemDto>> GetById(
        int id,
        [FromQuery] int userId)
    {
        if (id <= 0 || userId <= 0)
            return BadRequest("ID and User ID must be greater than 0.");

        var item = await _service.GetByIdAsync(id, userId);

        if (item == null)
            return NotFound();

        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<WatchlistItemDto>> Create(
        CreateWatchlistItemDto dto,
        [FromQuery] int userId)
    {
        if (userId <= 0)
            return BadRequest("User ID must be greater than 0.");

        var item = await _service.CreateAsync(dto, userId);

        if (item == null)
            return Conflict("User not found or media already exists.");

        return CreatedAtAction(
            nameof(GetById),
            new { id = item.Id, userId },
            item);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<WatchlistItemDto>> Update(
        int id,
        UpdateWatchlistItemDto dto,
        [FromQuery] int userId)
    {
        if (id <= 0 || userId <= 0)
            return BadRequest("ID and User ID must be greater than 0.");

        var item = await _service.UpdateAsync(id, dto, userId);

        if (item == null)
            return NotFound();

        return Ok(item);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        [FromQuery] int userId)
    {
        if (id <= 0 || userId <= 0)
            return BadRequest("ID and User ID must be greater than 0.");

        var deleted = await _service.DeleteAsync(id, userId);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}