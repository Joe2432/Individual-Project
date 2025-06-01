public static class PlayMatchMapper
{
    public static PlayMatchViewModel ToViewModel(PlayMatchDto dto)
    {
        return new PlayMatchViewModel
        {
            MatchId = dto.MatchId,
            MatchType = dto.MatchType,
            PartnerName = dto.PartnerName,
            Opponent1 = dto.Opponent1,
            Opponent2 = dto.Opponent2,
            Surface = dto.Surface,
            Status = dto.Status
        };
    }
}
