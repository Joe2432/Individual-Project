public static class UserEntityMapper
{
    public static UserCreateDto ToCreateDto(this UserEntity entity)
    {
        return new UserCreateDto
        {
            Username = entity.Username,
            Email = entity.Email,
            Password = entity.PasswordHash, // Note: hashed already
            Age = entity.Age,
            Gender = entity.Gender
        };
    }

    public static UserEntity ToEntity(this UserCreateDto dto, string hashedPassword)
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
