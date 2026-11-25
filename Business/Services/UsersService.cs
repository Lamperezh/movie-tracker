using Business.DTOs.User;
using Business.Models;
using Business.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public class UsersService : IUsersService
{
    private readonly UserManager<User> _userManager;

    public UsersService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        return users.Select(ToDto);
    }

    public async Task<UserDto> GetByIdAsync(int id)
    {
        if (id <= 0) throw new ArgumentException("User ID must be greater than 0.");

        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null) throw new KeyNotFoundException("User not found.");

        return ToDto(user);
    }

    public async Task<UserDto> UpdateAsync(int id, UpdateUserDto dto)
    {
        if (id <= 0) throw new ArgumentException("User ID must be greater than 0.");

        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null) throw new KeyNotFoundException("User not found.");

        var username = dto.UserName.Trim();
        var email = dto.Email.Trim();

        var existingUsername = await _userManager.FindByNameAsync(username);
        if (existingUsername != null && existingUsername.Id != id) throw new InvalidOperationException("Username is already taken.");

        var existingEmail = await _userManager.FindByEmailAsync(email);
        if (existingEmail != null && existingEmail.Id != id) throw new InvalidOperationException("Email is already registered.");

        user.UserName = username;
        user.Email = email;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded) throw new ArgumentException(string.Join("; ", result.Errors.Select(e => e.Description)));

        return ToDto(user);
    }

    public async Task DeleteAsync(int id)
    {
        if (id <= 0) throw new ArgumentException("User ID must be greater than 0.");

        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null) throw new KeyNotFoundException("User not found.");

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded) throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
    }

    private static UserDto ToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty
        };
    }
}