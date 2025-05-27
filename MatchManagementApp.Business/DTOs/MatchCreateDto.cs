public class MatchCreateDto
{
    public int CreatedByUserId { get; set; }
    public string MatchType { get; set; } = string.Empty;
    public string? PartnerName { get; set; }
    public string FirstOpponentName { get; set; } = string.Empty;
    public string? SecondOpponentName { get; set; }
    public int NrSets { get; set; }
    public string FinalSetType { get; set; } = string.Empty;
    public string GameFormat { get; set; } = string.Empty;
    public string Surface { get; set; } = string.Empty;
}
