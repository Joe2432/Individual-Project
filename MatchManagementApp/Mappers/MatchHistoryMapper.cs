public static class MatchHistoryMapper
{
    public static MatchHistoryViewModel ToViewModel(MatchHistoryDto dto)
    {
        return new MatchHistoryViewModel
        {
            Match = dto.Match,
            ScoreSummary = dto.ScoreSummary,
            MatchOver = dto.MatchOver
        };
    }
}
