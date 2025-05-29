public class UserDto
{
    public int? Id { get; set; }

    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Age { get; set; }
    public string? Gender { get; set; }
    public string? Password { get; set; }
    public string? PasswordHash { get; set; }
}
