using System.Security.Claims;
using System.Threading.Tasks;

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
}
