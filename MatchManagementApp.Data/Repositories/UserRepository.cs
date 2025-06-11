using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserDto?> GetUserByEmailAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        return user?.ToCreateDto();
    }


    public async Task<UserDto?> GetUserByUsernameAsync(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        return user?.ToCreateDto();
    }

    public async Task<UserDto?> GetUserByIdAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        return user?.ToCreateDto();
    }

    public async Task<int> CreateUserAsync(UserDto dto)
    {
        var entity = new UserEntity(dto.Username, dto.Email, dto.PasswordHash, dto.Age, dto.Gender);
        await _context.Users.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task DeleteUserAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateUserImageAsync(int userId, byte[] imageData)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            throw new Exception($"User with ID {userId} not found.");

        user.UpdateImage(imageData);

        await _context.SaveChangesAsync();
    }



}
