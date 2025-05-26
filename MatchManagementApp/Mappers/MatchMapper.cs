public static class MatchMapper
{
    public static MatchCreateDto ToCreateDto(MatchCreationViewModel vm, int userId)
    {
        return new MatchCreateDto
        {
            CreatedByUserId = userId,
            MatchType = vm.MatchType,
            PartnerName = vm.PartnerName,
            FirstOpponentName = vm.FirstOpponentName,
            SecondOpponentName = vm.SecondOpponentName,
            NrSets = vm.NrSets,
            FinalSetType = vm.FinalSetType,
            GameFormat = vm.GameFormat,
            Surface = vm.Surface
        };
    }
}