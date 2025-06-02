using MatchManagementApp.UI.ViewModels;

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
            WinnersPlayerA = dto.WinnersPlayerA,
            WinnersPlayerB = dto.WinnersPlayerB,
            UnforcedErrorsPlayerA = dto.UnforcedErrorsPlayerA,
            UnforcedErrorsPlayerB = dto.UnforcedErrorsPlayerB,
            ForcedErrorsPlayerA = dto.ForcedErrorsPlayerA,
            ForcedErrorsPlayerB = dto.ForcedErrorsPlayerB,
            TotalPointsPlayerA = dto.TotalPointsPlayerA,
            TotalPointsPlayerB = dto.TotalPointsPlayerB,
            TotalGamesPlayerA = dto.TotalGamesPlayerA,
            TotalGamesPlayerB = dto.TotalGamesPlayerB
        };
    }
}