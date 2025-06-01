using Xunit;
using Moq;

namespace MatchManagementApp.Tests.Services;

public class MatchServiceTests
{
    private readonly Mock<IMatchRepository> _matchRepositoryMock;
    private readonly Mock<IPointRepository> _pointRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly MatchService _matchService;

    public MatchServiceTests()
    {
        _matchRepositoryMock = new Mock<IMatchRepository>();
        _pointRepositoryMock = new Mock<IPointRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();

        _matchService = new MatchService(
            _matchRepositoryMock.Object,
            _pointRepositoryMock.Object,
            _userRepositoryMock.Object
        );
    }

    [Fact]
    public async Task CreateMatchAsync_ShouldReturnNewMatchId_WhenUserExists()
    {
        var dto = MatchDtoFactory.ValidSinglesMatch();
        var userDto = UserDtoFactory.ValidUser(dto.CreatedByUserId);

        _userRepositoryMock.Setup(r => r.GetUserByIdAsync(dto.CreatedByUserId)).ReturnsAsync(userDto);
        _matchRepositoryMock.Setup(r => r.CreateMatchAsync(dto)).ReturnsAsync(101);

        var result = await _matchService.CreateMatchAsync(dto);

        Assert.Equal(101, result);
    }

    [Fact]
    public async Task CreateMatchAsync_ShouldThrow_WhenUserNotFound()
    {
        var dto = MatchDtoFactory.ValidSinglesMatch();
        _userRepositoryMock.Setup(r => r.GetUserByIdAsync(dto.CreatedByUserId)).ReturnsAsync((UserDto?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _matchService.CreateMatchAsync(dto));
    }

    [Fact]
    public async Task GetUserMatchesAsync_ShouldReturnAllMatchesForUser()
    {
        int userId = 1;
        var matchList = new List<MatchDto>
        {
            MatchDtoFactory.ValidSinglesMatch(userId),
            MatchDtoFactory.ValidDoublesMatch(userId)
        };

        _matchRepositoryMock.Setup(r => r.GetMatchesByUserAsync(userId)).ReturnsAsync(matchList);

        var result = await _matchService.GetUserMatchesAsync(userId);

        Assert.Equal(2, result.Count);
        Assert.All(result, m => Assert.Equal(userId, m.CreatedByUserId));
    }

    [Fact]
    public async Task GetScoreDisplayAsync_ShouldReturnFormattedScore()
    {
        int matchId = 1, userId = 1;
        var match = MatchDtoFactory.ValidSinglesMatch(userId);
        var points = PointDtoFactory.SimulatedGamePoints(true);

        _matchRepositoryMock.Setup(r => r.GetMatchByIdAsync(matchId)).ReturnsAsync(match);
        _pointRepositoryMock.Setup(r => r.GetPointsByMatchIdAsync(matchId)).ReturnsAsync(points);

        var result = await _matchService.GetScoreDisplayAsync(matchId, userId);

        Assert.Contains("-", result);
    }

    [Fact]
    public async Task GetScoreDisplayAsync_ShouldReturnErrorMessage_WhenMatchNotFound()
    {
        int matchId = 999, userId = 1;
        _matchRepositoryMock.Setup(r => r.GetMatchByIdAsync(matchId)).ReturnsAsync((MatchDto?)null);

        var result = await _matchService.GetScoreDisplayAsync(matchId, userId);

        Assert.Equal("Match not found", result);
    }

    [Fact]
    public async Task RegisterPointAsync_ShouldAddPoint_WhenMatchExists()
    {
        int matchId = 1, userId = 1;
        var match = MatchDtoFactory.ValidSinglesMatch(userId);

        _matchRepositoryMock.Setup(r => r.GetMatchByIdAsync(matchId)).ReturnsAsync(match);

        await _matchService.RegisterPointAsync(matchId, userId, "User", true);

        _pointRepositoryMock.Verify(r => r.AddPointAsync(It.Is<PointDto>(p =>
            p.MatchId == matchId &&
            p.WinnerLabel == "User" &&
            p.IsServe == true
        )), Times.Once);
    }

    [Fact]
    public async Task RegisterPointAsync_ShouldThrow_WhenMatchDoesNotExist()
    {
        int matchId = 999;

        _matchRepositoryMock.Setup(r => r.GetMatchByIdAsync(matchId)).ReturnsAsync((MatchDto?)null);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _matchService.RegisterPointAsync(matchId, userId: 1, winnerLabel: "User", isServe: true));
    }

    [Fact]
    public async Task UndoLastPointAsync_ShouldCallDeleteLastPoint()
    {
        int matchId = 1;

        await _matchService.UndoLastPointAsync(matchId);

        _pointRepositoryMock.Verify(r => r.RemoveLastPointAsync(matchId), Times.Once);
    }

    [Fact]
    public async Task GetPlayMatchDtoAsync_ShouldReturnDto_WhenMatchExists()
    {
        int matchId = 1, userId = 1;
        var match = MatchDtoFactory.ValidSinglesMatch(userId);
        var points = PointDtoFactory.SimulatedGamePoints(true);

        _matchRepositoryMock.Setup(r => r.GetMatchByIdAsync(matchId)).ReturnsAsync(match);
        _pointRepositoryMock.Setup(r => r.GetPointsByMatchIdAsync(matchId)).ReturnsAsync(points);

        var result = await _matchService.GetPlayMatchDtoAsync(matchId, userId);

        Assert.NotNull(result);
        Assert.NotNull(result.Score);
        Assert.True(result.Score.SetScores.Count > 0);
    }

    [Fact]
    public async Task GetPlayMatchDtoAsync_ShouldReturnNull_WhenMatchNotFound()
    {
        int matchId = 123, userId = 1;
        _matchRepositoryMock.Setup(r => r.GetMatchByIdAsync(matchId)).ReturnsAsync((MatchDto?)null);

        var result = await _matchService.GetPlayMatchDtoAsync(matchId, userId);

        Assert.Null(result);
    }
}
