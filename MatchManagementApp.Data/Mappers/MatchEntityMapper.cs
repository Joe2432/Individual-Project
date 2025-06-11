public static class MatchEntityMapper
{
    public static MatchEntity ToEntity(this MatchDto dto)
    {
        return new MatchEntity(
            dto.CreatedByUserId,
            dto.MatchType,
            dto.FirstOpponentName,
            dto.NrSets,
            dto.FinalSetType,
            dto.GameFormat,
            dto.Surface,
            dto.InitialServer,
            dto.PartnerName,
            dto.SecondOpponentName
        );
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
            MatchDate = entity.MatchDate,
            InitialServer = entity.InitialServer
        };
    }
}
