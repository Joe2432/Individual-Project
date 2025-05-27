using System.Security.Claims;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;

    public UserService(IUserRepository userRepository, IAuthService authService) // <-- Add IAuthService here
    {
        _userRepository = userRepository;
        _authService = authService;
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
    public async Task<SignInResult> TrySignInAsync(string username, string password)
    {
        var userEntity = await _userRepository.GetUserByUsernameAsync(username);
        if (userEntity == null || !VerifyPassword(userEntity.PasswordHash, password))
        {
            return new SignInResult { Success = false, ErrorMessage = "Invalid username or password." };
        }
        var userDto = new UserCreateDto
        {
            Id = userEntity.Id,
            Username = userEntity.Username,
            Email = userEntity.Email,
            PasswordHash = userEntity.PasswordHash,
            Age = userEntity.Age,
            Gender = userEntity.Gender
        };
        var claimsPrincipal = _authService.GetClaimsPrincipal(userDto);
        return new SignInResult { Success = true, ClaimsPrincipal = claimsPrincipal };
    }
    public async Task<RegisterResult> TryRegisterAsync(UserCreateDto userDto)
    {
        var existingUser = await _userRepository.GetUserByUsernameAsync(userDto.Username);
        if (existingUser != null)
        {
            return new RegisterResult { Success = false, ErrorMessage = "Username is already taken." };
        }

        await CreateUserAsync(userDto);

        var user = await _userRepository.GetUserByUsernameAsync(userDto.Username);
        if (user == null)
        {
            return new RegisterResult { Success = false, ErrorMessage = "User creation failed." };
        }

        var claimsPrincipal = _authService.GetClaimsPrincipal(user);
        return new RegisterResult { Success = true, ClaimsPrincipal = claimsPrincipal };
    }
    public async Task<UserReadDto?> GetUserByIdAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
            return null;

        return new UserReadDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Age = user.Age,
            Gender = user.Gender
        };
    }
}
