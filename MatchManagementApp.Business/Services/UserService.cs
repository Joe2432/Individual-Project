using System.Security.Claims;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserCreateDto?> GetUserByEmailAsync(string email)
    {
        return await _userRepository.GetUserByEmailAsync(email);
    }

    public async Task<UserCreateDto?> GetUserByUsernameAsync(string username)
    {
        return await _userRepository.GetUserByUsernameAsync(username);
    }

    public async Task<UserCreateDto?> GetUserEntityByUsernameAsync(string username)
    {
        return await _userRepository.GetUserByUsernameAsync(username);
    }

    public async Task CreateUserAsync(UserCreateDto dto)
    {
        dto.PasswordHash = HashPassword(dto.Password);
        await _userRepository.CreateUserAsync(dto);
    }

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string hash, string raw)
    {
        return BCrypt.Net.BCrypt.Verify(raw, hash);
    }

    public Task<int?> GetCurrentUserId(ClaimsPrincipal principal)
    {
        var idClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(idClaim, out var id) ? Task.FromResult<int?>(id) : Task.FromResult<int?>(null);
    }

    public async Task<UserCreateDto> GetUserViewModelAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        return user ?? new UserCreateDto();
    }

    public async Task DeleteUserAsync(int userId)
    {
        await _userRepository.DeleteUserAsync(userId);
    }
}
