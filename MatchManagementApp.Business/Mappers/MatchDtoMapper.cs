public static class MatchDtoMapper
{
    public static MatchDto FromModel(Match model)
    {
        return new MatchDto
        {
            Id = model.Id,
            MatchType = model.MatchType,
            PartnerName = model.PartnerName,
            FirstOpponentName = model.FirstOpponentName,
            SecondOpponentName = model.SecondOpponentName,
            Surface = model.Surface,
            NrSets = model.NrSets,
            FinalSetType = model.FinalSetType,
            GameFormat = model.GameFormat,
            CreatedByUserId = model.CreatedByUserId,
            MatchDate = model.MatchDate
        };
    }
    public static Match ToModel(MatchDto dto)
    {
        return new Match(
            Id: dto.Id,
            createdByUserId: dto.CreatedByUserId,
            matchType: dto.MatchType,
            firstOpponentName: dto.FirstOpponentName,
            surface: dto.Surface,
            nrSets: dto.NrSets,
            finalSetType: dto.FinalSetType,
            gameFormat: dto.GameFormat,
            partnerName: dto.PartnerName,
            secondOpponentName: dto.SecondOpponentName,
            matchDate: dto.MatchDate
        );
    }

}
