public static class PlayMatchMapper
{
    public static PlayMatchViewModel ToViewModel(PlayMatchDto dto)
    {
        return new PlayMatchViewModel
        {
            Score = dto.Score,
            DisplaySetIndices = dto.DisplaySetIndices,
            UserSetGames = dto.UserSetGames,
            OpponentSetGames = dto.OpponentSetGames,
            GameUserDisplay = dto.GameUserDisplay,
            GameOpponentDisplay = dto.GameOpponentDisplay
        };
    }
}
