using System.Security.Claims;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;

    public UserService(IUserRepository userRepository, IAuthService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
    }

    public Task<UserDto?> GetUserByEmailAsync(string email)
        => _userRepository.GetUserByEmailAsync(email);

    public Task<UserDto?> GetUserByUsernameAsync(string username)
        => _userRepository.GetUserByUsernameAsync(username);

    public Task<UserDto?> GetUserEntityByUsernameAsync(string username)
        => _userRepository.GetUserByUsernameAsync(username);

    public async Task CreateUserAsync(UserDto dto)
    {
        dto.PasswordHash = HashPassword(dto.Password);
        await _userRepository.CreateUserAsync(dto);
    }

    public string HashPassword(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);

    public bool VerifyPassword(string hash, string raw)
        => BCrypt.Net.BCrypt.Verify(raw, hash);

    public Task<int?> GetCurrentUserIdAsync(ClaimsPrincipal principal)
    {
        var idClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(idClaim, out var id)
            ? Task.FromResult<int?>(id)
            : Task.FromResult<int?>(null);
    }

    public async Task<UserDto> GetUserViewModelAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        return user ?? new UserDto();
    }

    public Task DeleteUserAsync(int userId)
        => _userRepository.DeleteUserAsync(userId);

    public async Task<AuthResultDto> TrySignInAsync(string username, string password)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);
        if (user is null || user.PasswordHash is null || !VerifyPassword(user.PasswordHash, password))
        {
            return new AuthResultDto
            {
                Success = false,
                ErrorMessage = "Invalid username or password."
            };
        }

        var principal = _authService.GetClaimsPrincipal(user);
        return new AuthResultDto
        {
            Success = true,
            ClaimsPrincipal = principal
        };
    }

    public async Task<AuthResultDto> TryRegisterAsync(UserDto userDto)
    {
        var existing = await _userRepository.GetUserByUsernameAsync(userDto.Username);
        if (existing is not null)
        {
            return new AuthResultDto
            {
                Success = false,
                ErrorMessage = "Username is already taken."
            };
        }

        await CreateUserAsync(userDto);

        var created = await _userRepository.GetUserByUsernameAsync(userDto.Username);
        if (created is null)
        {
            return new AuthResultDto
            {
                Success = false,
                ErrorMessage = "User creation failed."
            };
        }

        var principal = _authService.GetClaimsPrincipal(created);
        return new AuthResultDto
        {
            Success = true,
            ClaimsPrincipal = principal
        };
    }

    public async Task UpdateUserImageAsync(int userId, byte[] imageData)
    {
        await _userRepository.UpdateUserImageAsync(userId, imageData);
    }


    public Task<UserDto?> GetUserByIdAsync(int userId)
        => _userRepository.GetUserByIdAsync(userId);
}
