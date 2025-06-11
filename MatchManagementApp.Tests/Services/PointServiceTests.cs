using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Moq;
using Xunit;


namespace MatchManagementApp.Tests.Services
{
    public class PointServiceTests
    {
        private readonly Mock<IPointRepository> _pointRepoMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly PointService _sut;

        public PointServiceTests()
        {
            _pointRepoMock = new Mock<IPointRepository>();
            _userServiceMock = new Mock<IUserService>();

            _sut = new PointService(
                _pointRepoMock.Object,
                _userServiceMock.Object
            );
        }

        [Fact]
        public async Task RegisterPointAsync_GetsUserIdThenAddsPoint()
        {
            // Arrange
            var dummyDto = PointDtoFactory.UserWin(type: "Winner", shots: 3);
            var fakeUser = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "123")
            }, "TestAuth"));

            _userServiceMock
                .Setup(u => u.GetCurrentUserIdAsync(fakeUser))
                .ReturnsAsync(123);

            // Act
            await _sut.RegisterPointAsync(dummyDto, fakeUser);

            // Assert
            _pointRepoMock.Verify(r => r.AddPointAsync(dummyDto), Times.Once);
        }

        [Fact]
        public async Task GetPointsForMatchAsync_ReturnsPointListFromRepo()
        {
            // Arrange
            var matchId = 88;
            var expectedPoints = new List<PointDto>
            {
                PointDtoFactory.UserWin(),
                PointDtoFactory.OpponentWin()
            };

            _pointRepoMock
                .Setup(r => r.GetPointsByMatchIdAsync(matchId))
                .ReturnsAsync(expectedPoints);

            var fakeUser = new ClaimsPrincipal(new ClaimsIdentity());

            // Act
            var result = await _sut.GetPointsForMatchAsync(matchId, fakeUser);

            // Assert
            Assert.Equal(expectedPoints, result);
            _pointRepoMock.Verify(r => r.GetPointsByMatchIdAsync(matchId), Times.Once);
        }
    }
}
