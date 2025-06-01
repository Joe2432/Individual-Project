public static class MatchHistoryDtoMapper
{
    public static MatchHistoryDto FromDto(MatchDto dto)
    {
        return new MatchHistoryDto
        {
            Match = dto,
            ScoreSummary = "",     
            MatchOver = false,
            MatchDate = dto.MatchDate
        };
    }

    public static MatchHistoryDto FromModel(Match model)
    {
        return new MatchHistoryDto
        {
            Match = MatchDtoMapper.FromModel(model),
            ScoreSummary = model.CalculateScore().GetScoreSummary(),
            MatchOver = model.IsOver(),
            MatchDate = model.MatchDate
        };
    }
}
