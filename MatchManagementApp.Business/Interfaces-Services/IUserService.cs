using System.Security.Claims;

public interface IUserService
{
    Task<UserDto?> GetUserByEmailAsync(string email);
    Task<UserDto?> GetUserByUsernameAsync(string username);
    Task<UserDto?> GetUserEntityByUsernameAsync(string username);
    Task CreateUserAsync(UserDto dto);
    string HashPassword(string password);
    bool VerifyPassword(string hash, string raw);
    Task<int?> GetCurrentUserIdAsync(ClaimsPrincipal principal);
    Task<UserDto> GetUserViewModelAsync(int userId);
    Task DeleteUserAsync(int userId);
    Task<AuthResultDto> TrySignInAsync(string username, string password);
    Task<AuthResultDto> TryRegisterAsync(UserDto userDto);
    Task<UserDto?> GetUserByIdAsync(int userId);
    Task UpdateUserImageAsync(int userId, byte[] imageData);
}
