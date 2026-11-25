using System.Security.Claims;
using Business.DTOs.User;
using Business.Services;
using Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace movie_tracker.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        return Ok(await _usersService.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetById(int id)
    {
        if (!CanAccessUser(id))
        {
            return Forbid();
        }

        return Ok(await _usersService.GetByIdAsync(id));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserDto>> Update(
        int id,
        UpdateUserDto dto)
    {
        if (!CanAccessUser(id))
        {
            return Forbid();
        }

        return Ok(await _usersService.UpdateAsync(id, dto));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(int id)
    {
        await _usersService.DeleteAsync(id);

        return NoContent();
    }

    private bool CanAccessUser(int id)
    {
        if (User.IsInRole("Administrator"))
        {
            return true;
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        return int.TryParse(userId, out var currentUserId)
               && currentUserId == id;
    }
}