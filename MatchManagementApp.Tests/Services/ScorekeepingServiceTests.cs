using Xunit;
using System.Collections.Generic;

namespace MatchManagementApp.Tests.Services
{
    public class ScorekeepingServiceTests
    {
        private readonly ScorekeepingService _scorekeepingService;

        public ScorekeepingServiceTests()
        {
            _scorekeepingService = new ScorekeepingService();
        }

        [Fact]
        public void CalculateScore_ShouldReturnZero_WhenNoPointsPlayed()
        {
            // Arrange
            var match = MatchDtoFactory.ValidSinglesMatch();
            var points = new List<PointDto>();

            // Act
            var result = _scorekeepingService.CalculateScore(match, points);

            // Assert
            Assert.Equal("0 - 0", result.CurrentGameScore);
            Assert.Single(result.SetScores);
            Assert.Equal(0, result.SetScores[0].Player1Games);
            Assert.Equal(0, result.SetScores[0].Player2Games);
            Assert.False(result.MatchOver);
        }

        [Fact]
        public void CalculateScore_ShouldCountPointsAndGamesCorrectly()
        {
            // Arrange
            var match = MatchDtoFactory.ValidSinglesMatch();
            match.GameFormat = "Ad";
            var points = new List<PointDto>
            {
                new() { IsUserWinner = true },
                new() { IsUserWinner = true },
                new() { IsUserWinner = true },
                new() { IsUserWinner = true }
            };

            // Act
            var result = _scorekeepingService.CalculateScore(match, points);

            // Assert
            Assert.Equal("0 - 0", result.CurrentGameScore);
            Assert.Equal(1, result.SetScores[0].Player1Games);
            Assert.Equal(0, result.SetScores[0].Player2Games);
        }
    }
}
