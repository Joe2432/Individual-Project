using System.Security.Claims;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace MatchManagementApp.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly UserService _sut;

        public UserServiceTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _authServiceMock = new Mock<IAuthService>();
            _sut = new UserService(
                _userRepoMock.Object,
                _authServiceMock.Object
            );
        }

        [Fact]
        public void HashPassword_AlwaysReturnsHashedString()
        {
            // Arrange
            var raw = "myPlainPassword";

            // Act
            var hashed = _sut.HashPassword(raw);

            // Assert
            Assert.NotNull(hashed);
            Assert.NotEqual(raw, hashed);
            Assert.StartsWith("$2a$", hashed);
        }

        [Fact]
        public void VerifyPassword_CorrectRawMatchesHash_ReturnsTrue()
        {
            // Arrange
            var raw = "secret123";
            var hash = BCrypt.Net.BCrypt.HashPassword(raw);

            // Act
            var result = _sut.VerifyPassword(hash, raw);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_IncorrectRaw_ReturnsFalse()
        {
            // Arrange
            var hash = BCrypt.Net.BCrypt.HashPassword("somePassword");

            // Act
            var result = _sut.VerifyPassword(hash, "wrongPassword");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task TrySignInAsync_InvalidUsername_ReturnsFailure()
        {
            // Arrange
            var username = "nonexistent";
            var password = "whatever";

            _userRepoMock
                .Setup(r => r.GetUserByUsernameAsync(username))
                .ReturnsAsync((UserDto)null);

            // Act
            var result = await _sut.TrySignInAsync(username, password);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid username or password.", result.ErrorMessage);
            Assert.Null(result.ClaimsPrincipal);
        }

        [Fact]
        public async Task TrySignInAsync_WrongPassword_ReturnsFailure()
        {
            // Arrange
            var username = "bob";
            var password = "wrongpass";

            var stored = UserDtoFactory.ValidUser(id: 10);
            stored.Username = username;
            stored.PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctpass");

            _userRepoMock
                .Setup(r => r.GetUserByUsernameAsync(username))
                .ReturnsAsync(stored);

            // Act
            var result = await _sut.TrySignInAsync(username, password);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid username or password.", result.ErrorMessage);
            Assert.Null(result.ClaimsPrincipal);
        }

        [Fact]
        public async Task TrySignInAsync_CorrectCredentials_ReturnsSuccessWithPrincipal()
        {
            // Arrange
            var username = "alice";
            var password = "alicePass";

            var stored = UserDtoFactory.ValidUser(id: 123);
            stored.Username = username;
            stored.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

            _userRepoMock
                .Setup(r => r.GetUserByUsernameAsync(username))
                .ReturnsAsync(stored);

            var fakePrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, stored.Id.ToString())
            }, "TestAuth"));

            _authServiceMock
                .Setup(a => a.GetClaimsPrincipal(stored))
                .Returns(fakePrincipal);

            // Act
            var result = await _sut.TrySignInAsync(username, password);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(fakePrincipal, result.ClaimsPrincipal);
        }

        [Fact]
        public async Task TryRegisterAsync_UsernameAlreadyTaken_ReturnsFailure()
        {
            // Arrange
            var newUser = UserDtoFactory.ValidUser(id: 0);
            newUser.Username = "existingUser";

            _userRepoMock
                .Setup(r => r.GetUserByUsernameAsync("existingUser"))
                .ReturnsAsync(UserDtoFactory.ValidUser(id: 5));

            // Act
            var result = await _sut.TryRegisterAsync(newUser);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Username is already taken.", result.ErrorMessage);
            Assert.Null(result.ClaimsPrincipal);
        }

        [Fact]
        public async Task GetCurrentUserIdAsync_WithValidPrincipal_ReturnsId()
        {
            // Arrange
            var id = 555;
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString())
            }));

            // Act
            var result = await _sut.GetCurrentUserIdAsync(principal);

            // Assert
            Assert.Equal(id, result.Value);
        }

        [Fact]
        public async Task GetCurrentUserIdAsync_InvalidClaim_ReturnsNull()
        {
            // Arrange
            var principal = new ClaimsPrincipal(new ClaimsIdentity());

            // Act
            var result = await _sut.GetCurrentUserIdAsync(principal);

            // Assert
            Assert.Null(result);
        }
    }
}
