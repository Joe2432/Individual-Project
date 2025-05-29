public static class UserDtoFactory
{
    public static UserDto ValidUser(int id = 1)
    {
        return new UserDto
        {
            Id = id,
            Username = "TestUser",
            Email = "test@example.com",
            Age = 25,
            Gender = "Male",
            Password = "Test123!",
            PasswordHash = "hashed_value"
        };
    }

    public static UserDto AnotherUser(int id = 2)
    {
        return new UserDto
        {
            Id = id,
            Username = "Opponent",
            Email = "opponent@example.com",
            Age = 27,
            Gender = "Male",
            Password = "Opponent123!",
            PasswordHash = "hashed_opponent"
        };
    }
}
