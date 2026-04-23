using ConstructionERP.DTOs.User;

namespace ConstructionERP.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(CreateUserDto dto);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(int id);
        Task<bool> UpdateUserAsync(int id, UpdateUserDto dto);
    }
}
