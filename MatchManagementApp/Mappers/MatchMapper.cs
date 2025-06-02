public static class MatchMapper
{
    public static MatchDto ToCreateDto(MatchCreationViewModel viewModel, int userId)
    {
        return new MatchDto
        {
            CreatedByUserId = userId,
            MatchType = viewModel.MatchType,
            PartnerName = viewModel.PartnerName,
            FirstOpponentName = viewModel.FirstOpponentName,
            SecondOpponentName = viewModel.SecondOpponentName,
            Surface = viewModel.Surface,
            NrSets = viewModel.NrSets,
            FinalSetType = viewModel.FinalSetType,
            GameFormat = viewModel.GameFormat,
            InitialServer = viewModel.InitialServer
        };
    }
}
