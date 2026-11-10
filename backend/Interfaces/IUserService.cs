using backend.DTOs;
using backend.Models;
namespace backend.Interfaces;

public interface IUserService
{
    
    Task<bool> Register(RegisterDto request);
    Task<LoginResponseDto?> ValidateLogin(LoginDto request);
    Task<List<UserDto>> GetAllUsersAsync();
    Task<User?> UpdateUser(int id, UpdateUserDto dto);
    Task<bool> DeleteUser(int id);

}