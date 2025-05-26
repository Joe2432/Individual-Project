public class UserRepository : IUserRepository
{
    private static readonly List<UserEntity> _users = new();
    private static int _idCounter = 1;

    public Task<UserEntity?> GetUserByEmailAsync(string email)
    {
        var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(user);
    }

    public Task<UserEntity?> GetUserByUsernameAsync(string username)
    {
        var user = _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(user);
    }

    public Task<UserEntity?> GetUserByIdAsync(int userId)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId);
        return Task.FromResult(user);
    }

    public Task CreateUserAsync(UserEntity user)
    {
        _users.Add(user);
        return Task.CompletedTask;
    }

    public Task DeleteUserAsync(UserEntity user)
    {
        _users.Remove(user);
        return Task.CompletedTask;
    }
}