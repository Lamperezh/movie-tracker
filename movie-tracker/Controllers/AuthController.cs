using Business.DTOs.Auth;
using Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace movie_tracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthRegisterDto dto)
    {
        var result = await _authService.RegisterAsync(dto);
        if (!result.Succeeded) return BadRequest(result.Errors);
        return Ok(new { message = "Користувача успішно створено" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthLoginDto dto)
    {
        var token = await _authService.LoginAsync(dto);
        if (token == null) return Unauthorized(new { message = "Невірний email або пароль" });
        return Ok(new { token });
    }
}