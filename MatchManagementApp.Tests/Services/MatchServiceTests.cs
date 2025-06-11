using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;


namespace MatchManagementApp.Tests.Services
{
    public class MatchServiceTests
    {
        private readonly Mock<IMatchRepository> _matchRepoMock;
        private readonly Mock<IPointRepository> _pointRepoMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IScorekeepingService> _scoreServiceMock;
        private readonly MatchService _sut;

        public MatchServiceTests()
        {
            _matchRepoMock = new Mock<IMatchRepository>();
            _pointRepoMock = new Mock<IPointRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _scoreServiceMock = new Mock<IScorekeepingService>();

            _sut = new MatchService(
                _matchRepoMock.Object,
                _pointRepoMock.Object,
                _userRepoMock.Object,
                _scoreServiceMock.Object
            );
        }

        [Fact]
        public async Task CreateMatchAsync_InvokesRepositoryAndReturnsId()
        {
            // Arrange
            var dummyMatch = MatchDtoFactory.ValidSinglesMatch(userId: 1);

            _matchRepoMock
                .Setup(r => r.CreateMatchAsync(It.IsAny<MatchDto>()))
                .ReturnsAsync(42);

            // Act
            var returnedId = await _sut.CreateMatchAsync(dummyMatch);

            // Assert
            Assert.Equal(42, returnedId);
            _matchRepoMock.Verify(r => r.CreateMatchAsync(dummyMatch), Times.Once);
        }

        [Fact]
        public async Task DeleteMatchAsync_InvokesRepository()
        {
            // Arrange
            var matchId = 99;

            // Act
            await _sut.DeleteMatchAsync(matchId);

            // Assert
            _matchRepoMock.Verify(r => r.DeleteMatchAsync(matchId), Times.Once);
        }

        [Fact]
        public async Task GetMatchByIdAsync_WhenMatchExists_ReturnsDto()
        {
            // Arrange
            var dummyMatch = MatchDtoFactory.ValidSinglesMatch(userId: 2);
            dummyMatch.Id = 7;

            _matchRepoMock
                .Setup(r => r.GetMatchByIdAsync(7))
                .ReturnsAsync(dummyMatch);

            // Act
            var result = await _sut.GetMatchByIdAsync(7);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(7, result.Id);
            _matchRepoMock.Verify(r => r.GetMatchByIdAsync(7), Times.Once);
        }

        [Fact]
        public async Task GetMatchByIdAsync_WhenNotFound_ReturnsNull()
        {
            // Arrange
            _matchRepoMock
                .Setup(r => r.GetMatchByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((MatchDto)null);

            // Act
            var result = await _sut.GetMatchByIdAsync(1234);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserMatchesAsync_InvokesScorekeepingAndPopulatesDisplayFields()
        {
            // Arrange
            var userId = 5;

            var match1 = MatchDtoFactory.ValidSinglesMatch(userId: userId);
            match1.Id = 1;
            match1.NrSets = 3;
            match1.GameFormat = "Standard";
            match1.FinalSetType = "Standard";
            match1.SetScores = new List<SetScoreDto>();
            match1.MatchDate = DateTime.UtcNow.AddDays(-1);
            match1.FirstOpponentName = "Alice";
            match1.SecondOpponentName = "Bob";

            var match2 = MatchDtoFactory.ValidSinglesMatch(userId: userId);
            match2.Id = 2;
            match2.NrSets = 1;
            match2.GameFormat = "Standard";
            match2.FinalSetType = "Standard";
            match2.SetScores = new List<SetScoreDto>();
            match2.MatchDate = DateTime.UtcNow;
            match2.FirstOpponentName = "Carol";
            match2.SecondOpponentName = "Dave";

            var userMatches = new List<MatchDto> { match1, match2 };
            _matchRepoMock
                .Setup(r => r.GetMatchesByUserIdAsync(userId))
                .ReturnsAsync(userMatches);

            _pointRepoMock
                .Setup(r => r.GetPointsByMatchIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<PointDto>());

            _scoreServiceMock
                .Setup(s => s.CalculateScore(It.IsAny<MatchDto>(), It.IsAny<IEnumerable<PointDto>>()))
                .Returns<MatchDto, IEnumerable<PointDto>>((m, pts) => m);

            // Act
            var results = await _sut.GetUserMatchesAsync(userId);

            // Assert
            Assert.Equal(2, results.Count);

            _scoreServiceMock.Verify(s => s.CalculateScore(
                It.Is<MatchDto>(m => m.Id == 1),
                It.IsAny<IEnumerable<PointDto>>()), Times.Once);
            _scoreServiceMock.Verify(s => s.CalculateScore(
                It.Is<MatchDto>(m => m.Id == 2),
                It.IsAny<IEnumerable<PointDto>>()), Times.Once);

            foreach (var m in results)
            {
                Assert.True(
                    m.GameUserDisplay == "0 - 0" ||
                    m.GameUserDisplay == m.CurrentGameScore
                );
                Assert.True(
                    m.GameOpponentDisplay == "0 - 0" ||
                    m.GameOpponentDisplay == m.CurrentGameScore
                );
            }
        }

        [Fact]
        public async Task GetScoreDisplayAsync_WhenMatchNotFound_ReturnsNotFoundMessage()
        {
            // Arrange
            _matchRepoMock
                .Setup(r => r.GetMatchByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((MatchDto)null);

            // Act
            var display = await _sut.GetScoreDisplayAsync(123, 5);

            // Assert
            Assert.Equal("Match not found", display);
        }

        [Fact]
        public async Task GetScoreDisplayAsync_WhenMatchExists_ReturnsCurrentGameScore()
        {
            // Arrange
            var matchId = 11;
            var dummyMatch = MatchDtoFactory.ValidSinglesMatch(userId: 5);
            dummyMatch.Id = matchId;

            _matchRepoMock
                .Setup(r => r.GetMatchByIdAsync(matchId))
                .ReturnsAsync(dummyMatch);

            _pointRepoMock
                .Setup(r => r.GetPointsByMatchIdAsync(matchId))
                .ReturnsAsync(new List<PointDto>());

            var scoredMatch = MatchDtoFactory.ValidSinglesMatch(userId: 5);
            scoredMatch.Id = matchId;
            scoredMatch.CurrentGameScore = "15 - 30";

            _scoreServiceMock
                .Setup(s => s.CalculateScore(It.IsAny<MatchDto>(), It.IsAny<IEnumerable<PointDto>>()))
                .Returns(scoredMatch);

            // Act
            var display = await _sut.GetScoreDisplayAsync(matchId, 5);

            // Assert
            Assert.Equal("15 - 30", display);
        }

        [Fact]
        public async Task RegisterPointAsync_CreatesNewDtoAndCallsPointRepo()
        {
            // Arrange
            var matchId = 77;
            var userId = 42;
            var pointType = "Ace";
            var isUserWinner = true;

            var dummyMatch = MatchDtoFactory.ValidSinglesMatch(userId: userId);
            dummyMatch.Id = matchId;
            _matchRepoMock
                .Setup(r => r.GetMatchByIdAsync(matchId))
                .ReturnsAsync(dummyMatch);

            // Act
            await _sut.RegisterPointAsync(matchId, userId, pointType, isUserWinner);

            // Assert
            _pointRepoMock.Verify(r => r.AddPointAsync(It.Is<PointDto>(dto =>
                dto.MatchId == matchId &&
                dto.PointType == pointType &&
                dto.IsUserWinner == isUserWinner &&
                dto.NumberOfShots == 0
            )), Times.Once);
        }

        [Fact]
        public async Task RegisterPointAsync_WhenMatchNull_DoesNothing()
        {
            // Arrange
            _matchRepoMock
                .Setup(r => r.GetMatchByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((MatchDto)null);

            // Act
            await _sut.RegisterPointAsync(123, 5, "Fault", false);

            // Assert
            _pointRepoMock.Verify(r => r.AddPointAsync(It.IsAny<PointDto>()), Times.Never);
        }

        [Fact]
        public async Task UndoLastPointAsync_InvokesDeleteLastPoint()
        {
            // Arrange
            var matchId = 55;

            // Act
            await _sut.UndoLastPointAsync(matchId);

            // Assert
            _pointRepoMock.Verify(r => r.DeleteLastPointAsync(matchId), Times.Once);
        }
    }
}
