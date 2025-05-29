public static class UserEntityMapper
{
    public static UserDto ToCreateDto(this UserEntity entity)
    {
        return new UserDto
        {
            Username = entity.Username,
            Email = entity.Email,
            Password = entity.PasswordHash, // Note: hashed already
            Age = entity.Age,
            Gender = entity.Gender
        };
    }

    public static UserEntity ToEntity(this UserDto dto, string hashedPassword)
    {
        return new UserEntity(
            dto.Username,
            dto.Email,
            hashedPassword,
            dto.Age,
            dto.Gender,
            "/images/default-user.png"
        );
    }


}
