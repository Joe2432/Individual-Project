public interface IUserRepository
{
    Task<UserCreateDto?> GetUserByEmailAsync(string email);
    Task<UserCreateDto?> GetUserByUsernameAsync(string username);
    Task<UserCreateDto?> GetUserByIdAsync(int userId);
    Task<int> CreateUserAsync(UserCreateDto dto);
    Task DeleteUserAsync(int userId);
}
