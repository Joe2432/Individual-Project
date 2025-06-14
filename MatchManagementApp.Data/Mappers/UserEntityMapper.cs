﻿public static class UserEntityMapper
{
    public static UserDto ToCreateDto(this UserEntity entity)
    {
        return new UserDto
        {
            Id = entity.Id,
            Username = entity.Username,
            Email = entity.Email,
            PasswordHash = entity.PasswordHash,
            Age = entity.Age,
            Gender = entity.Gender,
            ImageBytes = entity.ImageBytes
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
        null,
        "/images/default-user.png"
);

    }
}
