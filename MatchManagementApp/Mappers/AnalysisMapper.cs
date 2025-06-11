public static class AnalysisMapper
{
    public static AnalysisViewModel ToViewModel(AnalysisDto dto)
    {
        return new AnalysisViewModel
        {
            MatchId = dto.MatchId,
            PlayerAName = dto.PlayerAName,
            PlayerBName = dto.PlayerBName,
            MatchDate = dto.MatchDate,
            PlayerAGameByGame = dto.PlayerAGameByGame.ToList(),
            PlayerBGameByGame = dto.PlayerBGameByGame.ToList(),

            AcesPlayerA = dto.AcesPlayerA,
            AcesPlayerB = dto.AcesPlayerB,
            DoubleFaultsPlayerA = dto.DoubleFaultsPlayerA,
            DoubleFaultsPlayerB = dto.DoubleFaultsPlayerB,

            FirstServePctA = dto.FirstServePctA,
            FirstServePctB = dto.FirstServePctB,
            WinPctOnFirstServeA = dto.WinPctOnFirstServeA,
            WinPctOnFirstServeB = dto.WinPctOnFirstServeB,
            WinPctOnSecondServeA = dto.WinPctOnSecondServeA,
            WinPctOnSecondServeB = dto.WinPctOnSecondServeB,

            BreakPointsWonA = dto.BreakPointsWonA,
            BreakPointsOpportunitiesA = dto.BreakPointsOpportunitiesA,
            BreakPointsWonB = dto.BreakPointsWonB,
            BreakPointsOpportunitiesB = dto.BreakPointsOpportunitiesB,

            TiebreaksWonA = dto.TiebreaksWonA,
            TiebreaksWonB = dto.TiebreaksWonB,

            ReceivingPointsWonA = dto.ReceivingPointsWonA,
            ReceivingPointsWonB = dto.ReceivingPointsWonB,

            PointsWonA = dto.PointsWonA,
            PointsWonB = dto.PointsWonB,
            GamesWonA = dto.GamesWonA,
            GamesWonB = dto.GamesWonB,

            MaxGamesInARowA = dto.MaxGamesInARowA,
            MaxGamesInARowB = dto.MaxGamesInARowB,
            MaxPointsInARowA = dto.MaxPointsInARowA,
            MaxPointsInARowB = dto.MaxPointsInARowB,

            ServicePointsWonA = dto.ServicePointsWonA,
            ServicePointsWonB = dto.ServicePointsWonB,
            ServiceGamesWonA = dto.ServiceGamesWonA,
            ServiceGamesWonB = dto.ServiceGamesWonB,

            AvgShotsPerPoint = dto.AvgShotsPerPoint,
            MaxShotsInPoint = dto.MaxShotsInPoint,
        };
    }
}
