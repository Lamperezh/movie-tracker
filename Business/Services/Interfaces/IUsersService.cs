using Business.DTOs.User;

namespace Business.Services;

public interface IUsersService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto> GetByIdAsync(int id);
    Task<UserDto> UpdateAsync(int id, UpdateUserDto dto);
    Task DeleteAsync(int id);
}