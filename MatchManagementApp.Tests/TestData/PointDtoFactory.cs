public static class PointDtoFactory
{
    public static PointDto UserWin(string type = "Winner", int shots = 3)
    {
        return new PointDto
        {
            MatchId = 1,
            PointType = type,
            NumberOfShots = shots,
            IsUserWinner = true
        };
    }

    public static PointDto OpponentWin(string type = "ForcedError", int shots = 5)
    {
        return new PointDto
        {
            MatchId = 1,
            PointType = type,
            NumberOfShots = shots,
            IsUserWinner = false
        };
    }

    public static List<PointDto> SimulatedGamePoints(bool userWins)
    {
        return new List<PointDto>
        {
            new PointDto { MatchId = 1, IsUserWinner = userWins, PointType = "Ace", NumberOfShots = 1 },
            new PointDto { MatchId = 1, IsUserWinner = userWins, PointType = "Winner", NumberOfShots = 3 },
            new PointDto { MatchId = 1, IsUserWinner = userWins, PointType = "UnforcedError", NumberOfShots = 4 },
            new PointDto { MatchId = 1, IsUserWinner = userWins, PointType = "Winner", NumberOfShots = 2 },
        };
    }
}
