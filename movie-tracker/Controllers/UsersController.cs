
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using movie_tracker.DTOs.User;
using movie_tracker.Models;

namespace movie_tracker.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Administrator")]
public class UsersController : ControllerBase
{
    private readonly UserManager<User> _userManager;

    public UsersController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        var users = await _userManager.Users
            .Select(user => new UserDto
            {
                Id = user.Id,
                Username = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty
            })
            .ToListAsync();

        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetById(int id)
    {
        if (id <= 0)
            return BadRequest("User ID must be greater than 0.");

        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user == null)
            return NotFound();

        var userDto = new UserDto
        {
            Id = user.Id,
            Username = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty
        };

        return Ok(userDto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserDto>> Update(int id, UpdateUserDto dto)
    {
        if (id <= 0)
            return BadRequest("User ID must be greater than 0.");

        if (string.IsNullOrWhiteSpace(dto.UserName))
            return BadRequest("Username is required.");

        if (string.IsNullOrWhiteSpace(dto.Email))
            return BadRequest("Email is required.");

        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user == null)
            return NotFound();

        var username = dto.UserName.Trim();
        var email = dto.Email.Trim();

        var existingUsername = await _userManager.FindByNameAsync(username);

        if (existingUsername != null && existingUsername.Id != id)
            return Conflict("Username is already taken.");

        var existingEmail = await _userManager.FindByEmailAsync(email);

        if (existingEmail != null && existingEmail.Id != id)
            return Conflict("Email is already registered.");

        user.UserName = username;
        user.Email = email;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.Select(error => error.Description));
        }

        var userDto = new UserDto
        {
            Id = user.Id,
            Username = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty
        };

        return Ok(userDto);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
            return BadRequest("User ID must be greater than 0.");

        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user == null)
            return NotFound();

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.Select(error => error.Description));
        }

        return NoContent();
    }
}