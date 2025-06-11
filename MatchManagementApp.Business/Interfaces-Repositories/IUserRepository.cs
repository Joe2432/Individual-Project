using System.Threading.Tasks;

public interface IUserRepository
{
    Task<UserDto?> GetUserByEmailAsync(string email);
    Task<UserDto?> GetUserByUsernameAsync(string username);
    Task<UserDto?> GetUserByIdAsync(int userId);
    Task<int> CreateUserAsync(UserDto dto);
    Task DeleteUserAsync(int userId);
    Task UpdateUserImageAsync(int userId, byte[] imageData);
}
