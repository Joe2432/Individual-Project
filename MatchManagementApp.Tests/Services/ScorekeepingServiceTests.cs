using System.Collections.Generic;
using Xunit;

namespace MatchManagementApp.Tests.Services
{
    public class ScorekeepingServiceTests
    {
        private readonly ScorekeepingService _sut = new ScorekeepingService();

        [Fact]
        public void CalculateScore_BasicGame_WinnerGetsGame()
        {
            // Arrange
            var match = new MatchDto
            {
                Id = 1,
                NrSets = 1,
                GameFormat = "Standard",
                FinalSetType = "Standard",
                SetScores = new List<SetScoreDto>()
            };

            var points = new List<PointDto>
            {
                PointDtoFactory.UserWin(type: "Rally", shots: 1),
                PointDtoFactory.UserWin(type: "Rally", shots: 1),
                PointDtoFactory.UserWin(type: "Rally", shots: 1),
                PointDtoFactory.UserWin(type: "Rally", shots: 1),
            };

            // Act
            var result = _sut.CalculateScore(match, points);

            // Assert
            Assert.NotNull(result.SetScores);
            Assert.Single(result.SetScores);

            var firstSet = result.SetScores[0];
            Assert.Equal(1, firstSet.Player1Games);
            Assert.Equal(0, firstSet.Player2Games);
            Assert.Equal("0 - 0", result.CurrentGameScore);
            Assert.Contains("1-0", result.ScoreSummary);
        }

        [Fact]
        public void CalculateScore_TiebreakScenario_CorrectlyAssignsTiebreak()
        {
            // Arrange
            var match = new MatchDto
            {
                Id = 2,
                NrSets = 1,
                GameFormat = "Standard",
                FinalSetType = "Standard",
                SetScores = new List<SetScoreDto>()
            };

            var points = new List<PointDto>();

            for (int gameIndex = 0; gameIndex < 6; gameIndex++)
            {
                for (int i = 0; i < 4; i++)
                {
                    points.Add(PointDtoFactory.UserWin(type: "Rally", shots: 1));
                }
                for (int i = 0; i < 4; i++)
                {
                    points.Add(PointDtoFactory.OpponentWin(type: "Rally", shots: 1));
                }
            }

            for (int i = 0; i < 7; i++)
            {
                points.Add(PointDtoFactory.UserWin(type: "TiebreakPoint", shots: 1));
            }
            for (int i = 0; i < 5; i++)
            {
                points.Add(PointDtoFactory.OpponentWin(type: "TiebreakPoint", shots: 1));
            }

            // Act
            var result = _sut.CalculateScore(match, points);

            // Assert
            Assert.NotEmpty(result.SetScores);
            var firstSet = result.SetScores[0];

            Assert.Equal(7, firstSet.Player1Games);
            Assert.Equal(6, firstSet.Player2Games);

            Assert.Equal(0, firstSet.TiebreakScore);

            Assert.Equal("0 - 0", result.CurrentGameScore);

            Assert.Contains("7-6", result.ScoreSummary);
        }

        [Fact]
        public void CalculateScore_NoAdScoring_DecisivePoint_WorksCorrectly()
        {
            // Arrange
            var match = new MatchDto
            {
                Id = 3,
                NrSets = 1,
                GameFormat = "NoAd",
                FinalSetType = "Standard",
                SetScores = new List<SetScoreDto>()
            };

            var points = new List<PointDto>
            {
                PointDtoFactory.UserWin(type: "Rally", shots: 1),       
                PointDtoFactory.OpponentWin(type: "Rally", shots: 1),  
                PointDtoFactory.UserWin(type: "Rally", shots: 1),      
                PointDtoFactory.OpponentWin(type: "Rally", shots: 1),
                PointDtoFactory.UserWin(type: "Rally", shots: 1),
                PointDtoFactory.OpponentWin(type: "Rally", shots: 1),
                PointDtoFactory.UserWin(type: "Rally", shots: 1),
            };

            // Act
            var result = _sut.CalculateScore(match, points);

            // Assert
            Assert.NotEmpty(result.SetScores);
            var firstSet = result.SetScores[0];
            Assert.Equal(1, firstSet.Player1Games);
            Assert.Equal(0, firstSet.Player2Games);
            Assert.Equal("0 - 0", result.CurrentGameScore);
            Assert.Contains("1-0", result.ScoreSummary);
        }
    }
}
