﻿public class UserEntity
{
    public int Id { get; private set; }
    public string Username { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public int Age { get; private set; }
    public string Gender { get; private set; }
    public string ImageUrl { get; private set; }
    public byte[]? ImageBytes { get; private set; }

    public UserEntity(string username, string email, string passwordHash, int age, string gender, byte[]? imageBytes = null, string imageUrl = "/images/default-user.png")
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        Age = age;
        Gender = gender;
        ImageBytes = imageBytes;
        ImageUrl = imageUrl;
    }

    public void UpdateImage(byte[] imageData)
    {
        ImageBytes = imageData;
    }

    private UserEntity() { }
}
