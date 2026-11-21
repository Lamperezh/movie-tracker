using Microsoft.AspNetCore.Identity;
using movie_tracker.DTOs.Auth;

namespace movie_tracker.Services;

public interface IAuthService
{
    Task<IdentityResult> RegisterAsync(AuthRegisterDto dto);
    Task<string?> LoginAsync(AuthLoginDto dto);
}