using movie_tracker.DTOs.User;

namespace movie_tracker.Services;

public interface IUsersService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto> GetByIdAsync(int id);
    Task<UserDto> UpdateAsync(int id, UpdateUserDto dto);
    Task DeleteAsync(int id);
}