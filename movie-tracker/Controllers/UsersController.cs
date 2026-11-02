using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using movie_tracker.Data;
using movie_tracker.DTOs.User;
using movie_tracker.Models;

namespace movie_tracker.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        var users = await _context.Users
            .Select(user => new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            })
            .ToListAsync();

        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetById(int id)
    {
        if (id <= 0)
            return BadRequest("User ID must be greater than 0.");

        var user = await _context.Users
            .Where(user => user.Id == id)
            .Select(user => new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            })
            .FirstOrDefaultAsync();

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> Create(CreateUserDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.UserName))
            return BadRequest("UserName is required.");

        if (string.IsNullOrWhiteSpace(dto.Email))
            return BadRequest("Email is required.");

        var UserName = dto.UserName.Trim();
        var email = dto.Email.Trim();

        var UserNameExists = await _context.Users
            .AnyAsync(user => user.UserName == UserName);

        if (UserNameExists)
            return Conflict("UserName is already taken.");

        var emailExists = await _context.Users
            .AnyAsync(user => user.Email == email);

        if (emailExists)
            return Conflict("Email is already registered.");

        var user = new User
        {
            UserName = UserName,
            Email = email
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var userDto = new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email
        };

        return CreatedAtAction(
            nameof(GetById),
            new { id = user.Id },
            userDto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserDto>> Update(int id, UpdateUserDto dto)
    {
        if (id <= 0)
            return BadRequest("User ID must be greater than 0.");

        if (string.IsNullOrWhiteSpace(dto.UserName))
            return BadRequest("UserName is required.");

        if (string.IsNullOrWhiteSpace(dto.Email))
            return BadRequest("Email is required.");

        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return NotFound();

        var UserName = dto.UserName.Trim();
        var email = dto.Email.Trim();

        var UserNameExists = await _context.Users
            .AnyAsync(existing =>
                existing.Id != id &&
                existing.UserName == UserName);

        if (UserNameExists)
            return Conflict("UserName is already taken.");

        var emailExists = await _context.Users
            .AnyAsync(existing =>
                existing.Id != id &&
                existing.Email == email);

        if (emailExists)
            return Conflict("Email is already registered.");

        user.UserName = UserName;
        user.Email = email;

        await _context.SaveChangesAsync();

        var userDto = new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email
        };

        return Ok(userDto);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
            return BadRequest("User ID must be greater than 0.");

        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return NotFound();

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}