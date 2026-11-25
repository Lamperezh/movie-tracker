using Business.DTOs.Auth;
using Microsoft.AspNetCore.Identity;

namespace Business.Services.Interfaces;

public interface IAuthService
{
    Task<IdentityResult> RegisterAsync(AuthRegisterDto dto);
    Task<string?> LoginAsync(AuthLoginDto dto);
}