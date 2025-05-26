using MatchManagementApp.Core.DTOs;
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
        var user = await _userRepository.GetUserByEmailAsync(email);
        return user?.ToCreateDto();
    }

    public async Task<UserCreateDto?> GetUserByUsernameAsync(string username)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);
        return user?.ToCreateDto();
    }

    public async Task<UserEntity?> GetUserEntityByUsernameAsync(string username)
    {
        return await _userRepository.GetUserByUsernameAsync(username);
    }

    public async Task CreateUserAsync(UserCreateDto dto)
    {
        var hashedPassword = HashPassword(dto.Password);
        var entity = dto.ToEntity(hashedPassword);
        await _userRepository.CreateUserAsync(entity);
    }


    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string hash, string raw)
    {
        bool isValid = BCrypt.Net.BCrypt.Verify(raw, hash);
        return isValid;
    }

    public Task<int?> GetCurrentUserId(ClaimsPrincipal principal)
    {
        var idClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(idClaim, out int id))
            return Task.FromResult<int?>(id);
        return Task.FromResult<int?>(null);
    }

    public async Task<UserCreateDto> GetUserViewModelAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        return user?.ToCreateDto() ?? new UserCreateDto();
    }

    public async Task DeleteUserAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user != null)
            await _userRepository.DeleteUserAsync(user);
    }
}
