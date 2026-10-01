using Microsoft.AspNetCore.Mvc;
using movie_tracker.DTOs.User;
using movie_tracker.Models;

namespace movie_tracker.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private static readonly List<User> Users =
    [
        new User
        {
            Id = 1,
            Username = "john_doe",
            Email = "john@example.com"
        },
        new User
        {
            Id = 2,
            Username = "jane_smith",
            Email = "jane@example.com"
        }
    ];
    
    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetAll()
    {
        var users = Users.Select(user => new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email
        });

        return Ok(users);
    }
    
    [HttpGet("{id}")]
    public ActionResult<UserDto> GetById(int id)
    {
        var user = Users.FirstOrDefault(user => user.Id == id);

        if (user == null)
        {
            return NotFound();
        }

        var userDto = new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email
        };

        return Ok(userDto);
    }
    
    [HttpPost]
    public ActionResult<UserDto> Create(CreateUserDto dto)
    {
        var user = new User
        {
            Id = Users.Count == 0 ? 1 : Users.Max(user => user.Id) + 1,
            Username = dto.Username,
            Email = dto.Email
        };

        Users.Add(user);

        var userDto = new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email
        };

        return CreatedAtAction(
            nameof(GetById),
            new { id = user.Id },
            userDto);
    }
    
    [HttpPut("{id}")]
    public ActionResult<UserDto> Update(int id, UpdateUserDto dto)
    {
        var user = Users.FirstOrDefault(user => user.Id == id);

        if (user == null)
        {
            return NotFound();
        }

        user.Username = dto.Username;
        user.Email = dto.Email;

        var userDto = new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email
        };

        return Ok(userDto);
    }
    
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var user = Users.FirstOrDefault(user => user.Id == id);

        if (user == null)
        {
            return NotFound();
        }

        Users.Remove(user);

        return NoContent();
    }
}