public static class MatchHistoryMapper
{
    public static MatchHistoryViewModel ToViewModel(MatchDto dto)
    {
        return new MatchHistoryViewModel
        {
            MatchId = dto.Id,
            MatchType = dto.MatchType,
            PartnerName = dto.PartnerName ?? "-",
            FirstOpponentName = dto.FirstOpponentName,
            SecondOpponentName = dto.SecondOpponentName ?? "-",
            Surface = dto.Surface,
            ScoreSummary = dto.ScoreSummary ?? "-",
            MatchOver = dto.MatchOver,
            MatchDate = dto.MatchDate
        };
    }
}
