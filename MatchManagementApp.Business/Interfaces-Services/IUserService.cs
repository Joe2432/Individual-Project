using System.Security.Claims;
using System.Threading.Tasks;

public interface IUserService
{
    Task<UserDto?> GetUserByEmailAsync(string email);
    Task<UserDto?> GetUserByUsernameAsync(string username);
    Task<UserDto?> GetUserEntityByUsernameAsync(string username);
    bool VerifyPassword(string hash, string raw);
    Task CreateUserAsync(UserDto dto);
    Task<int?> GetCurrentUserId(ClaimsPrincipal principal);
    Task<UserDto> GetUserViewModelAsync(int userId);
    Task DeleteUserAsync(int userId);
    Task<AuthResultDto> TrySignInAsync(string username, string password);
    Task<AuthResultDto> TryRegisterAsync(UserDto userDto);
    Task<UserDto?> GetUserByIdAsync(int userId);
}
