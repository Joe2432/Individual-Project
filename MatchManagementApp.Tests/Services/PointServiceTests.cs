using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using Moq;

namespace MatchManagementApp.Tests.Services
{
    public class PointServiceTests
    {
        private readonly Mock<IPointRepository> _pointRepositoryMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly PointService _pointService;
        private readonly ClaimsPrincipal _mockUser;

        public PointServiceTests()
        {
            _pointRepositoryMock = new Mock<IPointRepository>();
            _userServiceMock = new Mock<IUserService>();

            _pointService = new PointService(_pointRepositoryMock.Object, _userServiceMock.Object);

            _mockUser = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1")
            }, "mock"));
        }

        [Fact]
        public async Task RegisterPointAsync_ShouldAddPoint_WhenUserIsAuthenticated()
        {
            var dto = PointDtoFactory.UserWin();
            _userServiceMock
                .Setup(u => u.GetCurrentUserIdAsync(_mockUser))
                .ReturnsAsync(1);

            await _pointService.RegisterPointAsync(dto, _mockUser);

            _pointRepositoryMock.Verify(r => r.AddPointAsync(dto), Times.Once);
        }

        [Fact]
        public async Task RegisterPointAsync_ShouldThrow_WhenUserNotAuthenticated()
        {
            var dto = PointDtoFactory.UserWin();
            var anonymousUser = new ClaimsPrincipal();
            _userServiceMock
                .Setup(u => u.GetCurrentUserIdAsync(anonymousUser))
                .ReturnsAsync((int?)null);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _pointService.RegisterPointAsync(dto, anonymousUser));
        }

        [Fact]
        public async Task GetPointsForMatchAsync_ShouldReturnPoints()
        {
            int matchId = 1;
            var expectedPoints = PointDtoFactory.SimulatedGamePoints(true);

            _pointRepositoryMock
                .Setup(r => r.GetPointsByMatchIdAsync(matchId))
                .ReturnsAsync(expectedPoints);

            var result = await _pointService.GetPointsForMatchAsync(matchId, _mockUser);

            Assert.Equal(expectedPoints.Count, result.Count);
            Assert.All(result, p => Assert.True(p.IsUserWinner));
        }
    }
}
