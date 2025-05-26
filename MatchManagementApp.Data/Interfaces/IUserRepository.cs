public interface IUserRepository
{
    Task<UserEntity?> GetUserByEmailAsync(string email);
    Task<UserEntity?> GetUserByUsernameAsync(string username);
    Task<UserEntity?> GetUserByIdAsync(int userId);
    Task CreateUserAsync(UserEntity user);
    Task DeleteUserAsync(UserEntity user);
}