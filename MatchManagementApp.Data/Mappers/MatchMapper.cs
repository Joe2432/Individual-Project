public static class MatchMapper
{
    public static MatchEntity ToEntity(this MatchDto dto)
    {
        var match = new MatchEntity(
            dto.CreatedByUserId,
            dto.MatchType,
            dto.FirstOpponentName,
            dto.NrSets,
            dto.FinalSetType,
            dto.GameFormat,
            dto.Surface,
            dto.PartnerName,
            dto.SecondOpponentName
        );

        return match;
    }

    public static MatchDto ToDto(this MatchEntity entity)
    {
        return new MatchDto
        {
            Id = entity.Id,
            CreatedByUserId = entity.CreatedByUserId,
            MatchType = entity.MatchType,
            PartnerName = entity.PartnerName,
            FirstOpponentName = entity.FirstOpponentName,
            SecondOpponentName = entity.SecondOpponentName,
            NrSets = entity.NrSets,
            FinalSetType = entity.FinalSetType,
            GameFormat = entity.GameFormat,
            Surface = entity.Surface,
            MatchDate = entity.MatchDate
        };
    }
}
