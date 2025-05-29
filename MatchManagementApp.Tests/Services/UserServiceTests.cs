using Xunit;
using Moq;
using System.Security.Claims;

namespace MatchManagementApp.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _authServiceMock = new Mock<IAuthService>();
        _userService = new UserService(_userRepoMock.Object, _authServiceMock.Object);
    }

    [Fact]
    public async Task TryRegisterAsync_ShouldReturnSuccess_WhenUserDoesNotExist()
    {
        // Arrange
        var dto = UserDtoFactory.ValidUser();

        _userRepoMock
            .Setup(r => r.GetUserByEmailAsync(dto.Email))
            .ReturnsAsync((UserDto?)null);

        _userRepoMock
            .SetupSequence(r => r.GetUserByUsernameAsync(dto.Username))
            .ReturnsAsync((UserDto?)null) // First check: username not taken
            .ReturnsAsync(dto);           // After creation, simulate success

        _userRepoMock
            .Setup(r => r.CreateUserAsync(dto))
            .ReturnsAsync(1);


        _authServiceMock
            .Setup(a => a.GetClaimsPrincipal(dto))
            .Returns(new ClaimsPrincipal());

        // Act
        var result = await _userService.TryRegisterAsync(dto);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.ClaimsPrincipal);
    }



    [Fact]
    public async Task TryRegisterAsync_ShouldFail_WhenUsernameAlreadyExists()
    {
        // Arrange
        var existingUser = UserDtoFactory.ValidUser();

        _userRepoMock.Setup(r => r.GetUserByUsernameAsync(existingUser.Username))
                     .ReturnsAsync(existingUser); // Conflict found

        // Act
        var result = await _userService.TryRegisterAsync(existingUser);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Username is already taken.", result.ErrorMessage);
    }

    [Fact]
    public async Task TrySignInAsync_ShouldReturnSuccess_WhenCredentialsAreValid()
    {
        // Arrange
        var user = UserDtoFactory.ValidUser();
        user.Password = "Test123";
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test123");

        _userRepoMock.Setup(r => r.GetUserByUsernameAsync(user.Username))
                     .ReturnsAsync(user);

        _authServiceMock.Setup(a => a.GetClaimsPrincipal(user))
                        .Returns(new System.Security.Claims.ClaimsPrincipal());

        // Act
        var result = await _userService.TrySignInAsync(user.Username, "Test123");

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.ClaimsPrincipal);
    }

    [Fact]
    public async Task TrySignInAsync_ShouldFail_WhenPasswordIsInvalid()
    {
        // Arrange
        var user = UserDtoFactory.ValidUser();
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword("correct_password");

        _userRepoMock.Setup(r => r.GetUserByUsernameAsync(user.Username))
                     .ReturnsAsync(user);

        // Act
        var result = await _userService.TrySignInAsync(user.Username, "wrong_password");

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Invalid username or password.", result.ErrorMessage);
    }

    [Fact]
    public void HashPassword_ShouldReturnHashedValue()
    {
        // Arrange
        var plain = "MyPass123";

        // Act
        var hash = _userService.HashPassword(plain);

        // Assert
        Assert.True(hash.StartsWith("$2")); // BCrypt hash
        Assert.True(BCrypt.Net.BCrypt.Verify(plain, hash));
    }
}
