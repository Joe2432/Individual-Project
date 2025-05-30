    public static class MatchHistoryMapper
    {
        public static MatchHistoryViewModel ToViewModel(MatchHistoryDto dto)
        {
            var match = dto.Match;

            return new MatchHistoryViewModel
            {
                MatchId = match.Id,
                MatchType = match.MatchType,
                PartnerName = string.IsNullOrWhiteSpace(match.PartnerName) ? "-" : match.PartnerName,
                FirstOpponentName = match.FirstOpponentName,
                SecondOpponentName = string.IsNullOrWhiteSpace(match.SecondOpponentName) ? "-" : match.SecondOpponentName,
                Surface = match.Surface,
                ScoreSummary = dto.ScoreSummary,
                MatchOver = dto.MatchOver,
                Status = dto.MatchOver ? "Completed" : "In Progress",
                MatchDate = dto.MatchDate
            };
        }
    }
