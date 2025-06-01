public static class MatchScoreDtoMapper
{
    public static MatchScoreDto FromModel(MatchScore model)
    {
        return new MatchScoreDto
        {
            UserSetsWon = model.UserSetsWon,
            OpponentSetsWon = model.OpponentSetsWon,
            SetScores = model.SetScores
                .Select(SetScoreDtoMapper.ToDto)
                .ToList(),
            CurrentGameScore = model.CurrentGameScore,
            InTiebreak = model.InTiebreak,
            MatchOver = model.MatchOver
        };
    }
}
