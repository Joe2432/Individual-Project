public static class MatchMapper
{
    public static MatchEntity ToEntity(this MatchCreateDto dto)
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

    public static MatchCreateDto ToDto(this MatchEntity entity)
    {
        return new MatchCreateDto
        {
            CreatedByUserId = entity.CreatedByUserId,
            MatchType = entity.MatchType,
            PartnerName = entity.PartnerName,
            FirstOpponentName = entity.FirstOpponentName,
            SecondOpponentName = entity.SecondOpponentName,
            NrSets = entity.NrSets,
            FinalSetType = entity.FinalSetType,
            GameFormat = entity.GameFormat,
            Surface = entity.Surface
        };
    }
}
