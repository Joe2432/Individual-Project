public static class PlayMatchMapper
{
    public static PlayMatchViewModel ToViewModel(MatchDto dto, List<PointDto> points, IServeStateService serveStateService)
    {
        var serveState = serveStateService.GetServeState(dto, points);

        return new PlayMatchViewModel
        {
            DisplaySetIndices = Enumerable.Range(0, dto.SetScores.Count).ToList(),
            UserSetGames = dto.SetScores.Select(s => s.Player1Games).ToList(),
            OpponentSetGames = dto.SetScores.Select(s => s.Player2Games).ToList(),
            GameUserDisplay = dto.GameUserDisplay,
            GameOpponentDisplay = dto.GameOpponentDisplay,
            MatchOver = dto.MatchOver,
            Score = new ScoreViewModel
            {
                InTiebreak = dto.InTiebreak,
                CurrentGameScore = dto.CurrentGameScore
            },
            CurrentServer = serveState.CurrentServer,
            IsFirstServe = serveState.IsFirstServe
        };
    }
}
