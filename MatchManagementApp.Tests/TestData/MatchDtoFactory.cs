public static class MatchDtoFactory
{
    public static MatchDto ValidSinglesMatch(int userId = 1)
    {
        return new MatchDto
        {
            CreatedByUserId = userId,
            MatchType = "Singles",
            Surface = "Clay",
            FirstOpponentName = "Nadal",
            NrSets = 3,
            FinalSetType = "Set",
            GameFormat = "Ad",
            MatchDate = DateTime.UtcNow
        };
    }

    public static MatchDto ValidDoublesMatch(int userId = 1)
    {
        return new MatchDto
        {
            CreatedByUserId = userId,
            MatchType = "Doubles",
            Surface = "Hard",
            FirstOpponentName = "Federer",
            SecondOpponentName = "Djokovic",
            PartnerName = "Murray",
            NrSets = 5,
            FinalSetType = "MaxiTiebreak",
            GameFormat = "NoAd",
            MatchDate = DateTime.UtcNow
        };
    }
}
