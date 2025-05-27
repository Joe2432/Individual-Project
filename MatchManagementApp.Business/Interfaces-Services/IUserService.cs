using System.Security.Claims;

public interface IUserService
{
    Task<UserCreateDto?> GetUserByEmailAsync(string email);
    Task<UserCreateDto?> GetUserByUsernameAsync(string username);
    Task<UserCreateDto?> GetUserEntityByUsernameAsync(string username);
    bool VerifyPassword(string hash, string raw);
    Task CreateUserAsync(UserCreateDto dto);
    Task<int?> GetCurrentUserId(ClaimsPrincipal principal);
    Task<UserCreateDto> GetUserViewModelAsync(int userId);
    Task DeleteUserAsync(int userId);
    Task<SignInResult> TrySignInAsync(string username, string password);
    Task<RegisterResult> TryRegisterAsync(UserCreateDto userDto);
    Task<UserReadDto?> GetUserByIdAsync(int userId);
}
